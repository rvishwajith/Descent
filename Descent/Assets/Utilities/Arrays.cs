using System;
using UnityEngine;

namespace Utilities
{
    public static class Arrays
    {
        public static int Wrap(int index, int arrLength)
        {
            return index % arrLength;
        }

        public static int WrapIndex(int index, Array a)
        {
            return Wrap(index, a.Length);
        }
    }
}



