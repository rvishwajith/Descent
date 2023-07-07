using UnityEngine;
using Utilities;

namespace Components
{
    public class Path : MonoBehaviour
    {
        public enum GizmoSettings { Always, Selected, Never }
        public GizmoSettings gizmoSettings = GizmoSettings.Selected;
        public bool useLabels = true;

        [HideInInspector]
        public Vector3[] points
        {
            get
            {
                var positions = new Vector3[transform.childCount];
                for (var i = 0; i < positions.Length; i++)
                    positions[i] = transform.GetChild(i).position;
                return positions;
            }
        }

        private void OnDrawGizmos()
        {
            if (gizmoSettings == GizmoSettings.Always)
                DrawGizmo();
        }

        private void OnDrawGizmosSelected()
        {
            if (gizmoSettings == GizmoSettings.Selected)
                DrawGizmo();
        }

        private void DrawGizmo()
        {
            Gizmos.color = Color.green;
            Labels.color = Color.green;

            for (var i = 0; i < points.Length; i++)
            {
                var point = points[i];
                var next = points[Math.Wrap(i + 1, points.Length)];

                Labels.World("Point " + i, point);
                Gizmos.DrawLine(point, next);
            }
        }
    }
}

