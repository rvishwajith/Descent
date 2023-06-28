using System;
using UnityEngine;

namespace Player
{
    public class Movement
    {
        private KeyCode moveKey = KeyCode.LeftShift;

        public bool useTouch = false;
        private float speed = 0, acceleration = 0.25f, deceleration = -1.25f;

        public float RelativeSpeed()
        {
            if (useTouch)
                TouchInput();
            else
            {
                TryGetTouchscreen();
                KeyboardInput();
            }
            speed = Mathf.Clamp(speed, 0, 1);
            return speed;
        }

        private void TryGetTouchscreen()
        {
            if (!useTouch && Input.touchCount > 0)
                useTouch = true;
        }

        private void TouchInput()
        {
            if (Input.touchCount >= 1)
                speed += acceleration * Time.deltaTime;
            else
                speed -= deceleration * Time.deltaTime;
        }

        private void KeyboardInput()
        {
            if (Input.GetKey(moveKey))
                speed += acceleration * Time.deltaTime;
            else
                speed += deceleration * Time.deltaTime;
        }
    }
}
