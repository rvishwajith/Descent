using System;
using UnityEngine;

/*
namespace Components.Player
{
    public class Movement
    {
        private Controller controller;

        private float acceleration { get { return controller.settings.idleToSwimAcceleration; } }
        private float deceleration { get { return controller.settings.swimDeceleration; } }

        private KeyCode key { get { return controller.settings.swimKey; } }

        public Movement(Controller controller)
        {
            this.controller = controller;
        }

        public bool checkForTouch = false;
        public bool useTouch = false;
        private float speed = 0;

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
                speed += deceleration * Time.deltaTime;
        }

        private void KeyboardInput()
        {
            if (Input.GetKey(key))
                speed += acceleration * Time.deltaTime;
            else
                speed += deceleration * Time.deltaTime;
        }
    }
}
*/