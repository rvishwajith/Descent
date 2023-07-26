using UnityEngine;
using UnityEditor;

namespace Components.Environment
{
    [CreateAssetMenu]
    public class WaterSettings : ScriptableObject
    {
        [Header("Ocean Color")]
        [SerializeField] public float surfaceHeight = 20;
        [SerializeField] public float lowestDepth = -500;
        [SerializeField] public Gradient depthToColorGradient;

        [Header("Underwater Visibility")]
        [SerializeField] AnimationCurve visibilityFalloffCurve;
        [SerializeField] public float visiblityNearPlane = 0.3f;
        [SerializeField] public float visibilityFarPlane = 500;

        [Header("Fog Rendering Options")]
        [SerializeField] public bool enableFogInSceneView = false;
        [SerializeField] public bool enableFogInGameView = false;
    }
}