using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerController controller;
    Transform body;
    Transform legLeft, legRight;

    float tLeg = 0;
    float tKnee;

    void Start()
    {
        FindChildren();
        controller = transform.GetComponent<PlayerController>();
        // AnimateBodyUpright();
    }

    void FindChildren()
    {
        body = transform.Find("Body");
        legLeft = body.Find("ThighL");
        legRight = body.Find("ThighR");
    }

    void Update()
    {
        if (controller.mode > -1)
        {
            AnimateLegs();
        }
    }

    void AnimateBodyUpright()
    {
        Delegates.Animation.Animate(
            body,
            property: "LocalEulerAngles",
            start: body.localEulerAngles,
            end: Vector3.down * 90,
            duration: 1.5f,
            delay: 0.25f,
            easing: 0);
    }

    void AnimateLegs()
    {
        var speed = Mathf.Clamp(Mathf.InverseLerp(0, controller.speedMax, controller.speed + 0.5f), 0, 1) + 0.5f;
        tLeg += 3.5f * speed * Time.deltaTime;

        var amp = 2.5f * speed + 0.5f;

        var tL = tLeg;
        var tR = tLeg + Mathf.PI;

        var offset = 5f;
        var sinL = amp * Mathf.Sin(tL) + offset;
        var sinR = amp * Mathf.Sin(tR) + offset;

        legLeft.localEulerAngles = Vector3.right * sinL;
        legRight.localEulerAngles = Vector3.right * sinR;

        AnimateKnee(legLeft.Find("Knee"), tL);
        AnimateKnee(legRight.Find("Knee"), tR);
    }

    void AnimateKnee(Transform knee, float t)
    {
        var amp = 6f;
        var offset = 3f;
        var sin = amp * Mathf.Sin(t) + offset;
        knee.localEulerAngles = Vector3.right * sin;
        AnimateAnkle(knee.Find("Ankle"), t);
    }

    void AnimateAnkle(Transform ankle, float t)
    {
        var amp = 5f;
        var offset = -20f;
        var sin = amp * Mathf.Sin(t) + offset;
        ankle.localEulerAngles = Vector3.right * sin;
    }
}