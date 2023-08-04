using UnityEngine;
using DG.Tweening;
using Utilities;

/* Components -> Performance -> DisplayMetrics
 * 
 * Class Description:
 * Displays realtime metrics on the UI, which are calculated using the serialized helper classes.
 * Available Metrics:
 * 1. Frames Per Second (Average / Minimum)
 * 2. Draw Calls (Average / Maximum)
 */

namespace Components.Performance
{
    public class DisplayPerformanceMetrics : MonoBehaviour
    {
        [SerializeField] private FPSMetrics FPS;

        private string fpsLabelContent = "Average FPS";

        private void Start()
        {
            DOVirtual.Float(0, 1, 1f / FPS.updatesPerSecond, _ => { })
                .SetLoops(-1)
                .OnStepComplete(() =>
                {
                    fpsLabelContent = "FPS (Average): " + Format.Float(FPS.averageFPS, 2);
                });
        }

        private void Update()
        {
            FPS.Sample();
        }

        private void OnGUI()
        {
            Draw.Label(new(Screen.width - 300, 20, 280, 50), fpsLabelContent);
        }
    }
}

