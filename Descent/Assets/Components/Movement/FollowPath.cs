using UnityEngine;
using DG.Tweening;

namespace Components.Movement
{
    public class FollowPath : MonoBehaviour
    {
        public Path path;

        [Header("Traversal Options")]
        [Range(3, 25)] public int samplingResolution = 10;
        public int startPoint = 0;

        [Header("Movement Options")]
        public MovementOptions movementType;
        public float travelSpeed = 20;

        private bool initialized { get { return path != null && path.points.Length > 4; } }

        private void Start()
        {
            if (!initialized)
                return;

            var points = path.WorldPositions(startPoint, looped: true);
            bool speedBased = movementType == MovementOptions.SpeedBased;

            transform.DOPath(
                path: points, duration: travelSpeed,
                pathType: PathType.CatmullRom, pathMode: PathMode.Full3D, resolution: 10,
                gizmoColor: Color.cyan)
                .SetSpeedBased(speedBased)
                .SetLoops(-1);
        }
    }

    public enum MovementOptions
    {
        SpeedBased,
        FixedDuration,
    }
}