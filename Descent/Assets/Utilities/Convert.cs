using UnityEngine;

namespace Utilities
{
    public static class Convert
    {
        static float DEG_PER_RAD = 180f / Mathf.PI;

        public static float DegToRad(float degrees)
        {
            return degrees / DEG_PER_RAD;
        }

        public static float RadToDeg(float radians)
        {
            return radians * DEG_PER_RAD;
        }
    }
}

