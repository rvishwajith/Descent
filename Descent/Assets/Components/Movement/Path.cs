using UnityEngine;
using Utilities;

namespace Components
{
    public class Path : MonoBehaviour
    {
        public Vector3[] points;

        [Header("Settings/Debug Options")]
        public DebugOptions gizmoMode = DebugOptions.Selected;
        public DebugOptions labelMode = DebugOptions.Selected;

        public Vector3[] WorldPoints(int startIndex = 0, bool looped = false)
        {
            var worldPoints = new Vector3[looped ? points.Length + 1 : points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                var localPoint = points[Arrays.WrapIndex(i + startIndex, points)];
                var worldPoint = transform.TransformPoint(localPoint);
                worldPoints[i] = worldPoint;
            }
            if (looped)
                worldPoints[^1] = points[Arrays.WrapIndex(startIndex, points)];
            return worldPoints;
        }

        private void OnDrawGizmos()
        {
            if (gizmoMode == DebugOptions.Always)
                DrawGizmos();
            if (labelMode == DebugOptions.Always)
                DrawLabels();
        }

        private void OnDrawGizmosSelected()
        {
            if (gizmoMode == DebugOptions.Selected)
                DrawGizmos();
            if (labelMode == DebugOptions.Selected)
                DrawLabels();
        }

        private void DrawGizmos()
        {
            if (points == null) return;

            Gizmos.color = Color.cyan;
            var worldPoints = WorldPoints();

            for (var i = 0; i < worldPoints.Length; i++)
            {
                Vector3 a = worldPoints[i], b = worldPoints[Arrays.WrapIndex(i + 1, worldPoints)];
                Gizmos.DrawLine(a, b);
            }
        }

        private void DrawLabels()
        {
            if (points == null) return;

            Labels.color = Color.cyan;
            var offset = Vector3.up / 2;
            var worldPoints = WorldPoints();

            for (var i = 0; i < worldPoints.Length; i++)
            {
                Labels.World("Point " + i, worldPoints[i] + offset);
            }
        }
    }
}

