using UnityEngine;

namespace Utilities
{
    public static class Format
    {
        public static string Float(float value, int decimals = -1)
        {
            if (decimals != -1)
                value = Math.Float.Round(value, decimals);
            return value + "";
        }

        public static string Vector(Vector3 value, int decimals = -1)
        {
            if (decimals != -1)
                value = Math.Vector.Round(value, decimals);
            float x = value.x, y = value.y, z = value.z;
            var result = "<" + x + "," + y + "," + z + ">";
            return result;
        }

        public static string Quaternion(Quaternion value, int decimals = -1)
        {
            if (decimals != -1)
                value = Math.Vector.Round(value, decimals);
            float x = value.x, y = value.y, z = value.z, w = value.w;
            var result = "<" + x + ", " + y + ", " + z + ", " + w + ">";
            return result;
        }

        public static string Array(float[] values, int decimals = -1)
        {
            var result = "[";
            for (var i = 0; i < values.Length; i++)
            {
                if (i > 0)
                    result += ", ";
                result += Format.Float(values[i], decimals: decimals);
            }
            result += "]";
            return result;
        }

        public static string Array(Vector3[] values, int decimals = -1)
        {
            var result = "[";
            for (var i = 0; i < values.Length; i++)
            {
                if (i > 0)
                    result += ", ";
                result += Format.Vector(values[i], decimals: decimals);
            }
            result += "]";
            return result;
        }
    }
}