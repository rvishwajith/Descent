using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Components.Rendering
{
    public class ManageRenderFeatures : MonoBehaviour
    {
        [SerializeField] private RenderFeatureToggle[] featureToggles;

        private void Start()
        {
            UpdateToggles();
        }

        private void FixedUpdate()
        {
            UpdateToggles();
        }

        /*
        private void OnDrawGizmos()
        {
            UpdateToggles();
        }

        public void OnValidate()
        {
            UpdateToggles();
        }
        */

        public void UpdateToggles()
        {
            if (featureToggles == null) return;

            foreach (var featureToggle in featureToggles)
            {
                featureToggle.gameCamera = GetComponentInParent<UnityEngine.Camera>();
                featureToggle.ToggleActive();
            }
        }
    }
}

namespace Components.Rendering
{
    [Serializable]
    public class RenderFeatureToggle
    {
        public ScriptableRendererFeature feature;
        public bool enableInScene = false;
        public bool enableInGame = true;
        [HideInInspector] public UnityEngine.Camera gameCamera;

        public void ToggleActive()
        {
            if (feature == null) return;

            var currentCamera = UnityEngine.Camera.current;

            if (!Application.isPlaying || gameCamera != currentCamera)
            {
                feature.SetActive(enableInScene);
            }
            else
            {
                feature.SetActive(enableInGame);
            }
        }
    }
}

