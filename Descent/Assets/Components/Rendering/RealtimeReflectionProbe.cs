using UnityEngine;
using UnityEngine.Rendering;

namespace Components.Rendering
{
    [RequireComponent(typeof(ReflectionProbe))]
    public class RealtimeReflectionProbe : MonoBehaviour
    {
        [SerializeField] public ReflectionProbe probe = null;
        private int renderID = -1;

        private void Start()
        {
            var probe = GetComponent<ReflectionProbe>();
            probe.mode = ReflectionProbeMode.Realtime;
            probe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            probe.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
            probe.renderDynamicObjects = true;
        }

        private void Update()
        {
            if (renderID == -1) return;

            if (renderID != -1 && probe.IsFinishedRendering(renderID))
            {
                RenderProbe();
                Debug.Log("Components.Lighting.RealtimeReflectionProbe(): Updated probe.");
            }
        }

        private void RenderProbe()
        {
            renderID = probe.RenderProbe(probe.realtimeTexture);
        }

        private void OnDrawGizmos()
        {
            if (probe == null)
                probe = GetComponent<ReflectionProbe>();
        }
    }
}