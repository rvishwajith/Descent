using System;
using UnityEngine;

namespace Components.Camera
{
    [CreateAssetMenu]
    public class CameraSettings : ScriptableObject
    {
        [Header("Camera Settings")]
        public float FOV = 60;
        public float zNear = 0.1f;
        public float zFar = 300f;

        [Header("Orbit Settings")]
        public CameraOrbitInputOptions orbitInputMode;
        public Vector2 orbitSensitivity = Vector2.one;
        public float defaultOrbitDistance = 3f;

        public ObservationMode observation;

        [Header("Input Settings")]
        public SwitchTargetInputOptions switchTargetInputMode;
        public KeyCode observationPreviousTargetKey = KeyCode.LeftArrow;
        public KeyCode observationNextTargetKey = KeyCode.RightArrow;
    }

    [Serializable]
    public class ObservationMode
    {
        [Header("Observation Mode")]
        [Range(0.05f, 5f)] public float smoothingFactor;
        [Range(0.05f, 5f)] public float transitionTime = 2.5f;
        [Range(0.05f, 20f)] public float transitionSpeed = 8f;
        public TransitionOptions transitionOptions;
    }

    public enum CameraOrbitInputOptions
    {
        AutoByPlatform,
        Any,
        Mouse,
        Keyboard,
        Touch
    }

    public enum SwitchTargetInputOptions
    {
        AutoByPlatform,
        Any,
        UI,
        Keyboard
    }

    public enum TransitionOptions
    {
        FixedDuration,
        BySpeed
    }
}