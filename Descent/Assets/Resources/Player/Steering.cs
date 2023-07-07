using System;
using UnityEngine;

namespace Player
{
    public class Steering
    {
        private Controller controller;

        private float Sensitivity
        {
            get
            {
                if (controller.settings.checkForGyroSteering && gyro != null)
                {
                    return controller.settings.gyroSteeringSensitivity;
                }
                else if (controller.settings.checkForKeyboardMovement)
                {
                    return controller.settings.keyboardSteeringSensitivity;
                }
                return controller.settings.mouseSteeringSensitivity;
            }
        }

        public Gyroscope gyro = null;
        private Quaternion initialGyroTilt = Quaternion.identity;
        public bool useGyro = false;
        private float
            gyroSens = 1f,
            gyroDiveMinAngle = 2,
            gyroDiveMaxAngle = 20,
            gyroTurnMinAngle = 3,
            gyroTurnMaxAngle = 35;

        public Vector3 Rotation
        {
            get
            {
                TryGyroscope();
                if (useGyro)
                    return GyroscopeInput();
                return MouseInput();
            }
        }

        public Steering(Controller controller)
        {
            this.controller = controller;
        }

        private Vector3 MouseInput()
        {
            var viewportPos = Camera.main.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
            var adjustedViewportPos = new Vector3(viewportPos.x - 0.5f, viewportPos.y - 0.5f) * 2;

            Vector3 turnStrength = new(-adjustedViewportPos.y, adjustedViewportPos.x);
            if (turnStrength.magnitude > 1)
                turnStrength = turnStrength.normalized;

            return turnStrength * Sensitivity;
        }

        private void TryGyroscope()
        {
            if (gyro == null && SystemInfo.supportsGyroscope)
            {
                Debug.Log("Gyroscope found!");
                gyro = UnityEngine.Input.gyro;
                gyro.enabled = true;
                gyro.updateInterval = 0.016f;
                useGyro = true;
                ResetGyroscopeTilt();
            }
        }

        Vector3 GyroscopeInput()
        {
            // To get A - B = A * Quaternion.Inverse(B)
            var absoluteTilt = gyro.attitude;
            var tilt = absoluteTilt * Quaternion.Inverse(initialGyroTilt);

            var diveAngle = tilt.eulerAngles.y;
            if (diveAngle >= 180)
                diveAngle -= 360;
            else if (diveAngle <= -180)
                diveAngle += 360;
            Debug.Log("Dive Angle: " + diveAngle);

            var turnAngle = tilt.eulerAngles.x;
            if (turnAngle >= 180)
                turnAngle -= 360;
            else if (turnAngle <= -180)
                turnAngle += 360;

            var diveStrength = Mathf.InverseLerp(gyroDiveMinAngle, gyroDiveMaxAngle, Mathf.Abs(diveAngle));
            if (diveAngle < 0)
                diveStrength *= -1;

            var turnStrength = Mathf.InverseLerp(gyroTurnMinAngle, gyroTurnMaxAngle, Mathf.Abs(turnAngle));
            if (turnAngle < 0)
                turnStrength *= -1;

            return new Vector3(diveStrength, turnStrength);
        }

        public void ResetGyroscopeTilt()
        {
            if (useGyro && gyro != null)
                initialGyroTilt = gyro.attitude;
        }
    }
}