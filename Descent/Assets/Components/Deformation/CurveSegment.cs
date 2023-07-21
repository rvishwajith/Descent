using UnityEngine;
using Utilities;

namespace Components.Deformation
{
    public class CurveSegment
    {
        public Transform a, b, c, d;
        public Vector3 p0, p1, p2, p3;
        public float length;

        public CurveSegment(Transform[] points)
        {
            this.a = points[0];
            this.b = points[1];
            this.c = points[2];
            this.d = points[3];
            Cache();
        }

        public void Cache(int lengthSamples = 20)
        {
            if (a == null || b == null || c == null || d == null)
            {
                Debug.Log("CurveSegment.Cache() ERROR: Some transform is null.");
                return;
            }
            p0 = a.position;
            p1 = b.position;
            p2 = c.position;
            p3 = d.position;
            length = GetLength(samples: lengthSamples);
        }

        public Vector3 Position(float t)
        {
            return Spline.Position(p0, p1, p2, p3, t);
        }

        public float GetLength(int samples = 20)
        {
            float approxLen = 0;
            float dT = 1f / samples;
            for (float t = 0; t <= 1; t += dT)
            {
                Vector3 prev = Position(t), curr = Position(t + dT);
                approxLen += (curr - prev).magnitude;
            }
            return approxLen;
        }

        public void DrawGizmos(int curveSamples = 20)
        {
            float dT = 1f / curveSamples;
            for (float t = dT; t <= 1; t += dT)
            {
                Vector3 prev = Position(t - dT), curr = Position(t);
                Gizmos.color = Color.Lerp(Color.blue, Color.white, t);
                Gizmos.DrawLine(prev, curr);
            }

            Gizmos.color = Color.gray;
            var pos = Position(0.5f);
            var offset = Vector3.up * 0.7f;
            Gizmo.Arrow(pos + offset, -offset);
            Labels.World("Length: " + Format.Float(length, 2), pos + offset);
        }
    }
}