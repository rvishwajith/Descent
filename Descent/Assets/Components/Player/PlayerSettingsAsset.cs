using UnityEngine;

namespace Components.Player
{
    [CreateAssetMenu]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Swimming")]
        public float swimSpeed = 8;
        public float idleToSwimAcceleration = 0.2f;
        public float swimToIdleAcceleration = -0.7f;
        [Range(0, 1)] public float swimToBoostSpeedThreshold = 0.98f;

        [Header("Boosting")]
        public float boostSpeed = 10;
        public float boostDuration = 3f;
        public float swimToBoostAcceleration = 1f;
        public float boostToSwimAcceleration = -0.7f;

        [Header("Steering")]
        public float diveMin = -85;
        public float diveMax = 85;
        public float steerTurnMultiplier = 75;
        public float steerDiveMultiplier = 50;

        [Header("Input Polling Options")]
        public MovementInputOptions movementInputOptions = MovementInputOptions.AutoByPlatform;
        public SteeringInputOptions steeringInputOptions = SteeringInputOptions.AutoByPlatform;

        [Header("Steering Input Multipliers")]
        public float steerKeyboardMultiplier = 2f;
        public float steerGyroscopeMultiplier = 1f;
        public float steerMouseMultiplier = 1f;

        [Header("Keyboard Input")]
        public KeyCode swimKey = KeyCode.LeftShift;
        public KeyCode boostKey = KeyCode.B;
        public KeyCode steerLeftKey = KeyCode.A;
        public KeyCode steerRightKey = KeyCode.D;
        public KeyCode steerUpKey = KeyCode.W;
        public KeyCode steerDownKey = KeyCode.S;
    }

    public enum MovementInputOptions
    {
        AutoByPlatform,
        Any,
        Touch,
        Mouse,
        Keyboard
    }

    public enum SteeringInputOptions
    {
        AutoByPlatform,
        Any,
        Gyroscope,
        Mouse,
        Keyboard
    }
}

