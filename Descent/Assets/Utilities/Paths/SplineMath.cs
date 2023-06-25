namespace Utilities
{
    namespace Paths
    {
        using UnityEngine;

        public static class Spline
        {
            public static float ApproximateLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int samples = 50)
            {
                float dT = 1f / samples, length = 0;
                for (float t = 0; t <= 1; t += dT)
                {
                    var prevPoint = Interpolate.Spline.Position(p0, p1, p2, p3, t - dT);
                    var currentPoint = Interpolate.Spline.Position(p0, p1, p2, p3, t);
                    var segmentLength = (currentPoint - prevPoint).magnitude;
                    length += segmentLength;
                }
                return length;
            }
        }
    }
}
