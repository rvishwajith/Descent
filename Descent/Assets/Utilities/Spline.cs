using UnityEngine;

namespace Utilities
{
    public static class Spline
    {
        public static int LENGTH_DEFAULT_SAMPLES = 50;

        public static float Length(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int samples = 0)
        {
            samples = samples == 0 ? LENGTH_DEFAULT_SAMPLES : samples;

            float dT = 1f / samples, length = 0;
            for (float t = 0; t <= 1; t += dT)
            {
                var prev = Interpolate.Spline.Position(p0, p1, p2, p3, t - dT);
                var curr = Interpolate.Spline.Position(p0, p1, p2, p3, t);
                var segmentLength = (curr - prev).magnitude;
                length += segmentLength;
            }
            return length;
        }

        public static float Length(Transform a, Transform b, Transform c, Transform d, int samples = 0)
        {
            return Length(a.position, b.position, c.position, d.position);
        }

        public static float Length(Vector3[] points, int samples = 0)
        {
            if (points.Length != 4)
            {
                Debug.Log("Splines.Length(Vector3[]) Error: Array has " + points.Length + " points, not 4.");
                return -1;
            }
            return Length(points[0], points[1], points[2], points[3], samples);
        }

        public static float Length(Transform[] transforms, int samples = 0)
        {
            if (transforms.Length != 4)
            {
                Debug.Log("Splines.Length(Transform[]) Error: Array has " + transforms.Length + " points, not 4.");
                return -1;
            }
            return Length(transforms[0], transforms[1], transforms[2], transforms[3], samples);
        }

        public static Vector3 Position(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var a = 2 * p1;
            var b = (-1 * p0 + p2) * t;
            var c = (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t;
            var d = (-1 * p0 + 3 * p1 - 3 * p2 + p3) * t * t * t;
            var point = 0.5f * (a + b + c + d);
            return point;
        }

        public static Vector3 Position(Transform a, Transform b, Transform c, Transform d, float t)
        {
            return Position(a.position, b.position, c.position, d.position, t);
        }

        public static Vector3 Position(Vector3[] points, float t)
        {
            if (points.Length != 4)
            {
                Debug.Log("Splines.Position(Vector3[]) Error: Array has " + points.Length + " points, not 4.");
                return Vector3.zero;
            }
            return Position(points[0], points[1], points[2], points[3], t);
        }

        public static Vector3 Position(Transform[] transforms, float t)
        {
            if (transforms.Length != 4)
            {
                Debug.Log("Splines.Position(Transform[]) Error: Array has " + transforms.Length + " points, not 4.");
                return Vector3.zero;
            }
            return Position(transforms[0], transforms[1], transforms[2], transforms[3], t);
        }
    }
}