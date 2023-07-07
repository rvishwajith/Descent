using UnityEngine;

namespace Utilities
{
    public static class Math
    {
        public static float Round(float value, float multiple = 1)
        {
            var remainder = value % multiple;
            var result = value - remainder;
            if (remainder >= (multiple / 2))
                result += multiple;
            return result;
        }

        public static Vector3 Round(Vector3 vector, float multiple = 1)
        {
            return new(
                Round(vector.x, multiple: multiple),
                Round(vector.y, multiple: multiple),
                Round(vector.z, multiple: multiple));
        }

        public static float RoundDown(float value, float multiple = 1)
        {
            var remainder = value % multiple;
            var result = value - remainder;
            if (value < 0 && remainder != 0)
                result -= multiple;
            return result;
        }

        public static Vector3 RoundDown(Vector3 vector, float multiple = 1)
        {
            return new(
                RoundDown(vector.x, multiple: multiple),
                RoundDown(vector.y, multiple: multiple),
                RoundDown(vector.z, multiple: multiple));
        }

        public static int Wrap(int index, int arrLength)
        {
            return index % arrLength;
        }
    }
}