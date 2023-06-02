using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PerformanceMetrics : MonoBehaviour
{
    [Header("Label (TextMeshPro)")]
    public TextMeshProUGUI label;

    FrameRateCalculator calculator = null;

    void Start()
    {
        calculator = new();
        StartCoroutine(UpdateLevel());
    }

    void Update()
    {
        calculator.Update(Time.deltaTime);
    }

    private IEnumerator UpdateLevel()
    {
        while (true)
        {
            label.text = calculator.ContextualFPS();
            yield return new WaitForSeconds(0.25f);
        }
        // yield return null;
    }
}

class FrameRateCalculator
{
    private Queue<float> times;
    private int maxSamples = 200, minSamples = 10;

    public FrameRateCalculator()
    {
        Debug.Log("Performance Metrics: Calculating FPS.");
        times = new();
    }

    public void Update(float deltaT)
    {
        times.Enqueue(deltaT);
        while (times.Count > maxSamples)
        {
            times.Dequeue();
        }
    }

    public float AverageFPS()
    {
        float numSamples = times.Count;
        float sumDeltaT = 0;
        foreach (float deltaT in times)
        {
            sumDeltaT += deltaT;
        }
        float meanDeltaT = sumDeltaT / numSamples;
        float meanFPS = 1f / meanDeltaT;
        return meanFPS;
    }

    public string ContextualFPS()
    {
        if (times.Count < minSamples)
        {
            return "FPS: N/A";
        }

        return "FPS: " + RoundFloat(AverageFPS(), 2);
    }

    public float RoundFloat(float value, int decimals)
    {
        int scaledValue = (int)(value * Mathf.Pow(10, decimals));
        float fpsRounded = (float)scaledValue / Mathf.Pow(10, decimals);
        return fpsRounded;
    }
}