using System;
using System.Collections;
using UnityEngine;

public class UniversalPlayerController : MonoBehaviour
{
    private PlayerControllerSteeringInput steeringInput;
    private Vector3 angles = Vector3.zero;
    private float turnSpeedMax = 75, diveSpeedMax = 45;

    private PlayerControllerMovementInput movementInput;
    private float speed = 0, moveSpeedMax = 7;

    private UniversalPlayerAnimator animator;
    private int state = 0;

    private void Start()
    {
        steeringInput = new();
        movementInput = new();
        animator = new(this.transform);

        StartCoroutine(animator.UprightState());
    }

    private void Update()
    {
        ApplyMovement();

        bool moving = (state == 1 || state == 2);

        if (!moving)
        {
            if (speed > 0)
            {
                StartCoroutine(animator.SwimmingState());
                NormalMovement();
                state = 1;
                Debug.Log("Switched state: Now swimming.");
            }
        }
        else if (moving) // Swimming
        {
            if (state == 1)
            {
                if (speed == 0)
                {
                    StartCoroutine(animator.UprightState());
                    state = 0;
                    Debug.Log("Switched state: Now idle.");
                }
                else if (speed > 0) ApplyRotation();
            }
        }
    }

    private void NormalMovement()
    {
        if (steeringInput.useGyroscope && Input.GetKey(KeyCode.Space))
        {
            steeringInput.ResetGyroscopeTilt();
            transform.rotation = Quaternion.identity;
            angles = Vector3.zero;
        }
    }

    private void ApplyMovement()
    {
        speed = Mathf.Lerp(0, moveSpeedMax, movementInput.GetRelativeSpeed());
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void ApplyRotation()
    {
        var rotation = Vector3.Scale(steeringInput.GetRotation(),
            new(diveSpeedMax, turnSpeedMax, 0));

        angles += rotation * Time.deltaTime;
        angles.x = Mathf.Clamp(angles.x, -70, 70);
        transform.rotation = Quaternion.Euler(angles.x, angles.y, 0);
    }
}

class UniversalPlayerAnimator
{
    private Transform body;
    private Transform headSwivel, head;

    public UniversalPlayerAnimator(Transform player)
    {
        body = player.Find("Body");

        headSwivel = body.Find("Skeleton/HeadSwivel");
        head = headSwivel.Find("Head");
    }

    public IEnumerator UprightState()
    {
        var delay = 0.015f;
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
        var delay = 0.015f;
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