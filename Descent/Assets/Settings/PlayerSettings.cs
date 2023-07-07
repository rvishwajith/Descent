using UnityEngine;

[CreateAssetMenu]
public class PlayerSettings : ScriptableObject
{
    [Header("Movement")]
    public float acceleration = 0.2f;
    public float deceleration = -0.7f;
    public float speedMax = 8;

    [Header("Steering")]
    public float diveMin = -85;
    public float diveMax = 85;
    public float turnMultiplier = 75;
    public float diveMultiplier = 50;

    [Header("Movement Input (Touch)")]
    public bool checkForTouchMovement = false;

    [Header("Movement Input (Keyboard)")]
    public bool checkForKeyboardMovement = false;
    public KeyCode moveKey = KeyCode.LeftShift;
    public KeyCode boostKey = KeyCode.B;

    [Header("Steering Input (Gyroscope)")]
    public bool checkForGyroSteering = false;
    public float gyroSteeringSensitivity = 1f;

    [Header("Steering Input (Keyboard)")]
    public bool checkForKeyboardSteering = false;
    public float keyboardSteeringSensitivity = 2f;

    [Header("Steering Input (Mouse)")]
    public bool checkForMouseSteering = true;
    public float mouseSteeringSensitivity = 1f;
}
