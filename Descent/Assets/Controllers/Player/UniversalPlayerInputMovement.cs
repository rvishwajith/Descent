using System;
using UnityEngine;

public class PlayerControllerMovementInput
{
    public bool useTouchscreen = false;

    private float
        speed = 0,
        acceleration = 0.2f,
        deceleration = 1.25f;

    private KeyCode moveKey = KeyCode.LeftShift;

    public float GetRelativeSpeed()
    {
        if (useTouchscreen)
            UseTouchscreenInput();
        else
        {
            TryGetTouchscreen();
            UseKeyboardInput();
        }
        speed = Mathf.Clamp(speed, 0, 1);
        return speed;
    }

    void TryGetTouchscreen()
    {
        if (!useTouchscreen && Input.touchCount > 0)
            useTouchscreen = true;
    }

    private void UseTouchscreenInput()
    {
        if (Input.touchCount >= 1)
            speed += acceleration * Time.deltaTime;
        else
            speed -= deceleration * Time.deltaTime;
    }

    private void UseKeyboardInput()
    {
        if (Input.GetKey(moveKey))
            speed += acceleration * Time.deltaTime;
        else
            speed -= deceleration * Time.deltaTime;
    }
}

