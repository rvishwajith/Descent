using UnityEngine;

namespace Species
{
    namespace Flocks
    {
        public struct VirtualBoidData
        {
            public Vector3 position;
            public Vector3 direction;

            public Vector3 flockHeading;
            public Vector3 flockCentre;
            public Vector3 avoidanceHeading;
            public int numFlockmates;

            public static int Size
            {
                get { return sizeof(float) * 3 * 5 + sizeof(int); }
            }
        }
    }
}