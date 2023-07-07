using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Life
{
    namespace Flocks
    {
        public static class Helper
        {
            // Default: 300.
            const int NUM_DIRECTIONS = 300;
            public static readonly Vector3[] DIRECTIONS;

            static Helper()
            {
                DIRECTIONS = new Vector3[Helper.NUM_DIRECTIONS];

                float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
                float angleIncrement = Mathf.PI * 2 * goldenRatio;

                for (int i = 0; i < NUM_DIRECTIONS; i++)
                {
                    float t = (float)i / NUM_DIRECTIONS;
                    float inclination = Mathf.Acos(1 - 2 * t);
                    float azimuth = angleIncrement * i;

                    float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
                    float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
                    float z = Mathf.Cos(inclination);
                    DIRECTIONS[i] = new Vector3(x, y, z);
                }
            }
        }
    }
}