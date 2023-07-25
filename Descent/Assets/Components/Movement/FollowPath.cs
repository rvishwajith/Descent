using UnityEngine;
using DG.Tweening;

namespace Components.Movement
{
    public class FollowPath : MonoBehaviour
    {
        public Path path;

        [Header("Traversal Options")]
        [Range(3, 20)] public int samplingResolution = 10;
        public int startPoint = 0;

        [Header("Movement Options")]
        public MovementOptions movementType;
        public float speed = 20;

        private bool speedBased { get { return movementType == MovementOptions.SpeedBased; } }
        private bool initialized { get { return path != null && path.points.Length >= 4; } }

        private void Start()
        {
            if (!initialized)
                return;

            var points = path.WorldPoints(startPoint, false);

            var tween = transform.DOPath(points, speed, PathType.CatmullRom, PathMode.Full3D,
                samplingResolution, Color.gray)
                .SetLookAt(lookAhead: 1, stableZRotation: true)
                .SetSpeedBased(speedBased)
                .SetEase(Ease.Linear)
                .SetLoops(-1);

            tween.OnStepComplete(() =>
            {
                // Debug.Log("Resetting back to destination!");
                tween.Pause();
                transform.DOMove(points[0], speed).SetSpeedBased(speedBased)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => { tween.Play(); });
            });
        }
    }

    public enum MovementOptions
    {
        SpeedBased,
        FixedDuration,
    }
}