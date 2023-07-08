using UnityEngine;

namespace Utilities
{
    public static class Format
    {
        public static string Vector(Vector3 value, int decimals = -1)
        {
            if (decimals != -1)
                value = Math.Vector.Round(value, decimals);
            float x = value.x, y = value.y, z = value.z;
            var result = "<" + x + ", " + y + ", " + z + ">";
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
    }
}

