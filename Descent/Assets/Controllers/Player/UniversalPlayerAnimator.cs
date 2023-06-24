using UnityEngine;
using System.Collections;
using System;

public class UniversalPlayerAnimator
{
    private UniversalPlayerController controller;

    private Transform body;
    private Transform headSwivel, head;

    private Transform thighL, thighR;
    private float thighT = 0, thighTOffset = Mathf.PI;

    public UniversalPlayerAnimator(UniversalPlayerController controller)
    {
        this.controller = controller;
        body = controller.transform.Find("Body");

        headSwivel = body.Find("Skeleton/HeadSwivel");
        head = headSwivel.Find("Head");

        thighL = body.Find("Skeleton/Waist/ThighL");
        thighR = body.Find("Skeleton/Waist/ThighR");
    }

    public void Update()
    {
        int state = controller.state;
        if (state == 1 || state == 2)
        {
            AnimateThighsSwimming();
        }
    }

    void AnimateThighsSwimming()
    {
        var speed = 0.5f + Mathf.Clamp(
            Mathf.InverseLerp(0, controller.moveSpeedMax + 0.5f, controller.speed), 0, 1);

        thighT += 3.5f * speed * Time.deltaTime;
        var amp = 2.5f * speed + 0.5f;

        var tL = thighT;
        var tR = thighT + thighTOffset;

        var offset = 5f;
        var sinL = amp * Mathf.Sin(thighT) + offset;
        var sinR = amp * Mathf.Sin(thighT) + offset;

        thighL.localEulerAngles = Vector3.right * sinL;
        thighR.localEulerAngles = Vector3.right * sinR;

        AnimateKneeSwimming(thighL.Find("Knee"), tL);
        AnimateKneeSwimming(thighR.Find("Knee"), tR);
    }

    void AnimateKneeSwimming(Transform knee, float t)
    {
        var amp = 6f;
        var offset = 3f;
        var sin = amp * Mathf.Sin(t) + offset;
        knee.localEulerAngles = Vector3.right * sin;
        AnimateAnkle(knee.Find("Ankle"), t);
    }

    void AnimateAnkle(Transform ankle, float t)
    {
        var amp = 10f;
        var offset = -35f;
        var sin = amp * Mathf.Sin(t) + offset;
        ankle.localEulerAngles = Vector3.right * sin;
    }

    public IEnumerator UprightState()
    {
        var delay = 0.0f;
        for (float t = 0; t <= 1; t += 1)
        {
            body.localEulerAngles = Vector3.right * Mathf.SmoothStep(body.localEulerAngles.x, -90, t);
            headSwivel.localEulerAngles = Vector3.right * Mathf.SmoothStep(headSwivel.localEulerAngles.x, 0, t);
            head.localEulerAngles = Vector3.right * Mathf.SmoothStep(head.localEulerAngles.x, 90, t);

            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    public IEnumerator SwimmingState()
    {
        var delay = 0.0f;
        for (float t = 0; t <= 1; t += 1)
        {
            body.localEulerAngles = Vector3.right * Mathf.SmoothStep(body.localEulerAngles.x, 0, t);
            headSwivel.localEulerAngles = Vector3.right * Mathf.SmoothStep(headSwivel.localEulerAngles.x, -30, t);
            head.localEulerAngles = Vector3.right * Mathf.SmoothStep(head.localEulerAngles.x, 45, t);

            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}

