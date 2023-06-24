using System;
using System.Collections;
using UnityEngine;

public class UniversalPlayerController : MonoBehaviour
{
    private PlayerControllerSteeringInput steeringInput;
    private Vector3 angles = Vector3.zero;
    private float turnSpeedMax = 75, diveSpeedMax = 45;

    private PlayerControllerMovementInput movementInput;
    [HideInInspector] public float speed = 0, moveSpeedMax = 8;

    private UniversalPlayerAnimator animator;
    [HideInInspector] public int state = 0; // -1 = Disabled, 0 = Idle, 1 = Swimming, 2 = Boosting

    private void Start()
    {
        steeringInput = new();
        movementInput = new();
        animator = new(this);

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
        animator.Update();
    }

    private void NormalMovement()
    {
        if (steeringInput.useGyroscope && Input.GetKey(KeyCode.R))
        {
            steeringInput.ResetGyroscopeTilt();
            transform.rotation = Quaternion.identity;
            angles = Vector3.zero;
        }
    }

    private void ApplyMovement()
    {
        speed = movementInput.GetRelativeSpeed() * moveSpeedMax;
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