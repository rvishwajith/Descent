using System;
using UnityEngine;

public class PlayerControllerSteeringInput
{
    private float mouseSensitivity = 1f;
    public bool useGyroscope = false;

    public Gyroscope gyroscope = null;
    private Quaternion initialGyroscopeTilt = Quaternion.identity;
    private float
        gyroscopeSensitivity = 1f,
        diveTiltMin = 3, diveTiltMax = 35,
        turnTiltMin = 3, turnTiltMax = 30;

    public Vector3 GetRotation()
    {
        if (useGyroscope)
            return GetGyroscopeRotation();

        TryGyroscope();
        return GetMouseInput();
    }

    private Vector3 GetMouseInput()
    {
        var viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        var adjustedViewportPos = new Vector3(viewportPos.x - 0.5f, viewportPos.y - 0.5f) * 2;

        Vector3 turnStrength = new(-adjustedViewportPos.y, adjustedViewportPos.x);
        if (turnStrength.magnitude > 1)
            turnStrength = turnStrength.normalized;

        return turnStrength * mouseSensitivity;
    }

    private void TryGyroscope()
    {
        if (gyroscope == null && SystemInfo.supportsGyroscope)
        {
            Debug.Log("Gyroscope found!");
            gyroscope = Input.gyro;
            gyroscope.enabled = true;
            gyroscope.updateInterval = 0.016f;
            // initialTilt = gyroscope.attitude;
        }
    }

    Vector3 GetGyroscopeRotation()
    {
        // To get A - B = A * Quaternion.Inverse(B)
        var absoluteTilt = gyroscope.attitude;
        var tilt = absoluteTilt * Quaternion.Inverse(initialGyroscopeTilt);

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

        var diveStrength = Mathf.InverseLerp(diveTiltMin, diveTiltMax, Mathf.Abs(diveAngle));
        if (diveAngle < 0)
            diveStrength *= -1;

        var turnStrength = Mathf.InverseLerp(turnTiltMin, turnTiltMax, Mathf.Abs(turnAngle));
        if (turnAngle < 0)
            turnStrength *= -1;

        return new Vector3(diveStrength, turnStrength);
    }

    public void ResetGyroscopeTilt()
    {
        if (useGyroscope && gyroscope != null)
            initialGyroscopeTilt = gyroscope.attitude;
    }
}