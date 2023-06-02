using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerController2 controller;
    Transform legLeft, legRight;

    float tLeg = 0;
    float tKnee;

    void Start()
    {
        FindChildren();
        controller = transform.GetComponent<PlayerController2>();
    }

    void FindChildren()
    {
        legLeft = transform.Find("Mesh/ThighL");
        legRight = transform.Find("Mesh/ThighR");
    }

    void Update()
    {
        if (controller.mode > -1)
        {
            AnimateLegs();
        }
    }

    void AnimateLegs()
    {
        var speed = Mathf.Clamp(Mathf.InverseLerp(0, controller.speedMax, controller.speed + 0.5f), 0, 1) + 0.5f;
        tLeg += 3.5f * speed * Time.deltaTime;

        var amp = 1.5f * speed + 0.5f;

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