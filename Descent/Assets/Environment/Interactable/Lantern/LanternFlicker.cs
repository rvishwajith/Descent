using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFlicker : MonoBehaviour
{
    public Transform bulb;
    private Material material;

    private static string EMISSION_KEYWORD = "_EmissionColor";
    private Color color = new(0.7f, 0.85f, 1f);

    private float onTime = 2f,
        onTimeNoise = 0.3f,
        onIntensity = 6f;
    private float offTime = 3f,
        offTimeNoise = 0.5f,
        offIntensity = 2f;

    private string state = "?";
    private float desiredIntensity,
        previousIntensity;
    private float timeSinceStateChange = -1,
        timeToStateChange = -1;

    private void Start()
    {
        material = bulb.GetComponent<MeshRenderer>().material;
        TurnLightOff();
    }

    private void Update()
    {
        timeSinceStateChange += Time.deltaTime;
        if (timeSinceStateChange <= timeToStateChange)  // State will not change. Animate here.
        {
            float multiplier = 1;
            if (state == "off")
            {
                multiplier = 1.5f;
            }
            else if (state == "on")
            {
                multiplier = 3f;
            }
            var t = Mathf.Clamp(multiplier * timeSinceStateChange / timeToStateChange, 0, 1);
            var intensity = Mathf.Lerp(previousIntensity, desiredIntensity, t);
            material.SetColor(LanternFlicker.EMISSION_KEYWORD, color * intensity);
        }
        else // State will change. Don't animate.
        {
            if (state == "on")
            {
                TurnLightOff();
            }
            else if (state == "off")
            {
                TurnLightOn();
            }
        }
    }

    private void TurnLightOn()
    {
        SetLightState("on");
        timeToStateChange = RandomChangeTime(onTime, onTimeNoise);
        desiredIntensity = onIntensity;
        previousIntensity = offIntensity;
    }

    private void TurnLightOff()
    {
        SetLightState("off");
        timeToStateChange = RandomChangeTime(offTime, offTimeNoise);
        desiredIntensity = offIntensity;
        previousIntensity = onIntensity;
    }

    private void SetLightState(string state)
    {
        // Debug.Log("SetLightState() - Setting state: " + state);
        this.state = state;
        timeSinceStateChange = 0;
    }

    private float RandomChangeTime(float time, float noise)
    {
        return time + (noise * (Random.value - 0.5f));
    }
}
