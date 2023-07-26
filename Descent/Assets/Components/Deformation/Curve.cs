using System;
using UnityEngine;
using Utilities;

namespace Components.Deformation
{
    public class CurveController
    {
        private CurveSegment[] segments;
        public float length;

        private float[] segmentRelLengths;
        private float[] segmentRelAggregates;

        public bool initialized = false;

        public CurveController(Transform[] points)
        {
            segments = new CurveSegment[points.Length - 3];
            for (var i = 0; i < segments.Length; i++)
            {
                segments[i] = new(points[i..(i + 4)]);
            }
            initialized = true;
        }

        public void Cache()
        {
            foreach (var segment in segments)
                segment.Cache();

            float GetLength()
            {
                float approxLen = 0;
                foreach (var segment in segments)
                    approxLen += segment.length;
                return approxLen;
            }
            float[] RelativeSegmentLengths(float totalLength)
            {
                var relativeLens = new float[segments.Length];
                for (var i = 0; i < segments.Length; i++)
                {
                    relativeLens[i] = segments[i].length / totalLength;
                }
                return relativeLens;
            }
            float[] RelativeSegmentAggregates(float[] relativeLens)
            {
                var aggregateLens = new float[segments.Length];
                float currAggregateLen = 0;
                for (var i = 0; i < relativeLens.Length; i++)
                {
                    currAggregateLen += relativeLens[i];
                    aggregateLens[i] = currAggregateLen;
                }
                return aggregateLens;
            }
            length = GetLength();
            segmentRelLengths = RelativeSegmentLengths(length);
            segmentRelAggregates = RelativeSegmentAggregates(segmentRelLengths);
        }

        public Vector3 Position(float t)
        {
            float segmentIndexAndOffset = LookupSegmentAndOffset(t);
            int segmentIndex = (int)segmentIndexAndOffset;
            float offset = segmentIndexAndOffset - segmentIndex;
            return segments[segmentIndex].Position(offset);
        }

        public void DrawGizmos(float gizmoT = 0.5f)
        {
            foreach (var segment in segments)
                segment.DrawGizmos();

            Vector3 origin = segments[0].Position(0),
                offset = Vector3.up * 1f;

            Gizmo.color = Color.gray;
            Labels.AtWorld("Total Length: " + Format.Float(length, 3), origin + offset);
            Gizmo.Arrow(origin + offset, -offset);

            offset = Vector3.up * -0.5f;
            Labels.AtWorld("Aggregate Lengths: " + Format.Array(segmentRelAggregates, 3), origin + offset);
            Gizmo.Arrow(origin + offset, -offset);

            /*
            offset = Vector3.up * -0.8f;
            origin = Position(gizmoT);
            Labels.World("Lookup Value: " + gizmoT + ", Result: " + LookupSegment(gizmoT), origin + pointOffset);
            Gizmo.Arrow(origin + pointOffset, -pointOffset);
            */
        }

        private float LookupSegmentAndOffset(float t)
        {
            return LookupSegment(0, segmentRelAggregates.Length - 1, t);
        }

        private float LookupSegment(int left, int right, float t)
        {
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (segmentRelAggregates[mid] == t) // Base case: C is present at middle
                    return mid;
                else if (segmentRelAggregates[mid] < t) // If x is greater, increment left half
                    left = mid + 1;
                else // If x is smaller, increment right half
                    right = mid - 1;
            }

            // Standard exit case (in between right and left, right = left - 1):
            var temp = left;
            left = right;
            right = temp;

            if (left <= 0)
                return 0;
            else if (right >= segmentRelAggregates.Length)
            {
                right = segmentRelAggregates.Length - 1;
                left = right - 1;
            }
            var offset = Mathf.InverseLerp(segmentRelAggregates[left], segmentRelAggregates[right], t);
            return right + offset;
            // Debug.Log("T = " + t + " between left = " + left + ", right = " + right + ", length = " + segmentRelAggregates.Length);
        }

        /*
        private int ExampleBinarySearch(float[] arr, int left, int right, float t)
        {
            while (left <= right)
            {
                int middle = left + (right - left) / 2;
                // Base case: C is present at middle
                if (arr[middle] == t)
                    return middle;
                // If x is greater, increment left half
                else if (arr[middle] < t)
                    left = middle + 1;
                // If x is smaller, increment right half
                else
                    right = middle - 1;
            }
            // If we reach here, then element was not present
            return -1;
        }
        */
    }
}

