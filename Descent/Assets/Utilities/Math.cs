using System;
using UnityEngine;

namespace Utilities
{
    public static class Math
    {
        public static class Vector
        {
            public static Vector2 Round(Vector2 vector, int decimals = 2)
            {
                float x = Float.Round(vector.x, decimals),
                    y = Float.Round(vector.y, decimals);
                return new(x, y);
            }

            public static Vector3 Round(Vector3 vector, int decimals = 2)
            {
                float x = Float.Round(vector.x, decimals),
                    y = Float.Round(vector.y, decimals),
                    z = Float.Round(vector.z, decimals);
                return new(x, y, z);
            }

            public static Quaternion Round(Quaternion value, int decimals = 2)
            {
                float x = Float.Round(value.x, decimals),
                    y = Float.Round(value.y, decimals),
                    z = Float.Round(value.z, decimals),
                    w = Float.Round(value.w, decimals);
                return new(x, y, z, w);
            }

            public static Vector3 Floor(Vector3 vector, float interval = 1)
            {
                float x = Float.Floor(vector.x, interval),
                    y = Float.Floor(vector.y, interval),
                    z = Float.Floor(vector.z, interval);
                return new(x, y, z);
            }
        }

        public static class Float
        {
            public static float Round(float value, int decimals = 2)
            {
                float multiplier = Mathf.Pow(10, decimals);
                return ((int)(value * multiplier) / multiplier);
            }

            public static float Floor(float value, float interval = 1)
            {
                var remainder = value % interval;
                var result = value - remainder;
                if (value < 0 && remainder != 0)
                    result -= interval;
                return result;
            }
        }

        public static class Arrays
        {
            public static int Wrap(int index, int length)
            {
                return Utilities.Arrays.Wrap(index, length);
            }

            public static int Wrap(int index, Array a)
            {
                return Utilities.Arrays.WrapIndex(index, a);
            }
        }

        // Artifact
        public static int Wrap(int index, int length)
        {
            return Utilities.Arrays.Wrap(index, length);
        }
    }
}