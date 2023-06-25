namespace ProceduralAnimation
{
    using UnityEngine;
    using Utilities;

    public class Spline
    {
        private Transform[] points;
        private Vector3 p0, p1, p2, p3;

        private Transform parent;
        private Vector3 parentDirection;
        private bool initialized = false;

        public Spline(Transform parent, Transform[] points)
        {
            this.points = points;
            this.parent = parent;

            initialized = points.Length >= 4;
            if (initialized)
                Update();
        }

        public void Update()
        {
            p0 = points[0].position;
            p1 = points[1].position;
            p2 = points[2].position;
            p3 = points[3].position;
            parentDirection = parent.forward;
        }

        public Vector3 Point(float t)
        {
            return Interpolate.Spline.Position(p0, p1, p2, p3, t);
        }

        public Vector3 ZTangent(float t)
        {
            float dT = 0.01f;
            var prevPoint = Point(t - dT);
            var nextPoint = Point(t + dT);
            var tangent = (prevPoint - nextPoint).normalized;
            return tangent;
        }

        public Vector3 XTangent(float t)
        {
            var forward = ZTangent(t);
            var tangent = -Vector3.Cross(forward, Vector3.up);
            return tangent;
        }

        public Vector3 YTangent(float t)
        {
            var forward = ZTangent(t);
            var tangent = Vector3.Cross(forward, Vector3.right);
            return tangent;
        }

        public float Length()
        {
            if (initialized)
                return Utilities.Paths.Spline.ApproximateLength(p0, p1, p2, p3);
            else
                return 0;
        }

        public void DrawGizmos()
        {
            if (!initialized) return;

            // Draw control points.
            Update();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p2, p3);

            // Draw spline segments.
            var length = Length();
            var dT = 1f / 20;
            for (var t = 0f; t <= 1; t += dT)
            {
                Gizmos.color = Color.Lerp(Color.yellow, Color.cyan, t);
                Gizmos.DrawLine(Point(t), Point(t + dT));
            }

            // Draw z tangents.
            dT = 1f / 15;
            Gizmos.color = Color.white;
            for (var t = dT; t <= 1; t += dT)
            {
                var prev = Point(t - dT);
                var point = Point(t);
                var rayLength = (prev - point).magnitude * 0.7f;
                var dir = ZTangent(t);
                Gizmos.DrawRay(point + parent.up, dir * rayLength);
                Gizmos.DrawSphere(point + parent.up, 0.03f);
            }

            // Draw x tangents.
            dT = 1f / 15;
            Gizmos.color = Color.red;
            for (var t = dT; t <= 1; t += dT)
            {
                var point = Point(t);
                Gizmos.DrawRay(point + parent.up, XTangent(t) * 0.4f);
                // Gizmos.DrawRay(Point(t - dT), Vector3.right);
            }

            // Draw y tangents.
            dT = 1f / 15;
            Gizmos.color = Color.yellow;
            for (var t = dT; t <= 1; t += dT)
            {
                var point = Point(t);
                Gizmos.DrawRay(point + parent.up, YTangent(t) * 0.4f);
                // Gizmos.DrawRay(Point(t - dT), Vector3.up)
            }
        }
    }
}