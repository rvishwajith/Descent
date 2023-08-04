using System;
using UnityEngine;

namespace Components.Camera
{
    [CreateAssetMenu(menuName = "Assets/Camera Settings")]
    public class CameraSettings : ScriptableObject
    {
        [Header("Camera Settings")]
        [SerializeField] public float FOV = 60;
        [SerializeField] public float zNear = 0.1f;
        [SerializeField] public float zFar = 300f;

        [Header("Orbit Settings")]
        [SerializeField] public CameraOrbitInputOptions orbitInputMode;
        [SerializeField] public Vector2 orbitSensitivity = Vector2.one;
        [SerializeField] public float defaultOrbitDistance = 3f;

        [SerializeField] public ObservationModeSettings observation;

        [Header("Input Settings")]
        [SerializeField] public SwitchTargetInputOptions switchTargetInputMode;
        [SerializeField] public KeyCode observationPreviousTargetKey = KeyCode.LeftArrow;
        [SerializeField] public KeyCode observationNextTargetKey = KeyCode.RightArrow;
    }

    [Serializable]
    public class ObservationModeSettings
    {
        [Header("Observation Mode")]
        [SerializeField] public float smoothingFactor = 0.5f;
        [SerializeField] public float transitionTime = 2.5f;
        [SerializeField] public float transitionSpeed = 8f;
        [SerializeField] public bool transitionBySpeed = false;
    }
}

namespace Components.Camera
{
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
}