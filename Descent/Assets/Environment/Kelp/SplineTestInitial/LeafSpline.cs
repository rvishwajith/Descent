namespace Kelp
{
    using UnityEngine;

    public class LeafSpline
    {
        private Transform[] points;
        private Vector3 p0, p1, p2, p3;

        public LeafSpline(Transform[] points)
        {
            this.points = points;
            UpdatePoints();
        }

        public void UpdatePoints()
        {
            p0 = points[0].position;
            p1 = points[1].position;
            p2 = points[2].position;
            p3 = points[3].position;
        }

        public Vector3 Position(float t)
        {
            return Interpolate.Spline.Position(p0, p1, p2, p3, t);
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.white;
            var tDelta = 0.02f;
            for (var t = tDelta; t <= 1; t += tDelta)
            {
                Gizmos.DrawLine(Position(t - tDelta), Position(t));
            }

            Gizmos.color = Color.yellow;
            for (var i = 0; i < points.Length; i++)
            {
                var radius = 0.05f;
                if (i == 0 || i == points.Length - 1)
                    radius = 0.03f;
                Gizmos.DrawWireSphere(points[i].position, radius);
            }
        }
    }
}