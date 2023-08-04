using System;
using UnityEngine;

namespace Components.Player
{
    [CreateAssetMenu(menuName = "Assets/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        public PlayerMovementSettings movement;

        [Header("Keyboard Input")]
        public KeyCode swimKey = KeyCode.Space;
        public KeyCode boostKey = KeyCode.B;
        public KeyCode steerLeftKey = KeyCode.A;
        public KeyCode steerRightKey = KeyCode.D;
        public KeyCode steerUpKey = KeyCode.W;
        public KeyCode steerDownKey = KeyCode.S;

        public Vector2 steeringSpeeds = new(30, 30);
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

namespace Components.Player
{
    [Serializable]
    public class PlayerMovementSettings
    {
        [Header("Swimming")]
        public AnimationCurve swimSpeedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float swimSpeedMax = 8;
        public float relSwimAcceleration = 0.5f;

        [Header("Steering")]
        public AnimationCurve steerSpeedMultiplierCurve = AnimationCurve.EaseInOut(0, 0.2f, 1, 1);
    }
}
