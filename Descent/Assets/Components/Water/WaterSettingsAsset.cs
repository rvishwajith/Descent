using UnityEngine;

namespace Components.Water
{
    [CreateAssetMenu(menuName = "Assets/Water Settings")]
    public class WaterSettings : ScriptableObject
    {
        [Header("Ocean Color")]
        [SerializeField] public float surfaceHeight = 20;
        [SerializeField] public float lowestDepth = -500;
        [SerializeField] public Gradient depthToColorGradient;

        [Header("Underwater")]
        [SerializeField] private BakeableShaderCurve underwaterVisibilityCurve;

        [Header("Camera")]
        [SerializeField] public float visiblityNearPlane = 0.3f;
        [SerializeField] public float visibilityFarPlane = 500;


        [Header("Render Settings")]
        [SerializeField] public bool enableFogInSceneView = false;
        [SerializeField] public bool enableFogInGameView = false;

        public void OnValidate()
        {
            underwaterVisibilityCurve.Update();
        }
    }
}