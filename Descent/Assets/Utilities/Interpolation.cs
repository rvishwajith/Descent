using System;
using UnityEngine;

public static class Interpolate
{
    public static class Spline
    {
        public static Vector3 Position(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var a = 2 * p1;
            var b = (-1 * p0 + p2) * t;
            var c = (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t;
            var d = (-1 * p0 + 3 * p1 - 3 * p2 + p3) * t * t * t;
            var point = 0.5f * (a + b + c + d);
            return point;
        }
    }

    public static class Polynomial
    {
        public static float Linear(float x)
        {
            return x;
        }

        public static float Quadratic(float x)
        {
            return Mathf.Pow(x, 2);
        }

        public static float Cubic(float x)
        {
            return Mathf.Pow(x, 3);
        }
    }

    public static class SmoothStep
    {
        public static float Point(float x)
        {
            return x;
        }
    }
}

