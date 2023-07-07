using UnityEngine;
using System.Collections.Generic;
using Utilities;

namespace Components
{
    namespace Deformation
    {
        class Curve
        {
            private DeformationController controller;
            private List<DeformationSpline> splines = new();
            private float length;

            public Curve(DeformationController controller)
            {
                // this.controller = controller;
                var transforms = controller.splineTransforms;
                for (var i = 3; i < transforms.Length; i++)
                {
                    DeformationSpline spline = new();
                    spline.tA = transforms[i - 3];
                    spline.tB = transforms[i - 2];
                    spline.tC = transforms[i - 1];
                    spline.tD = transforms[i];
                    splines.Add(spline);
                }
                Cache();
            }

            public void Cache()
            {
                float approxLength = 0;
                foreach (var spline in splines)
                {
                    spline.Cache();
                    approxLength += spline.length;
                }
                length = approxLength;
            }
        }

        struct DeformationSpline
        {
            public Transform tA, tB, tC, tD;
            public Vector3 a, b, c, d;
            public float length;

            public void Cache()
            {
                a = tA.position;
                b = tB.position;
                c = tC.position;
                d = tD.position;

                length = ApproximateLength();
            }

            public Vector3 Position(float t)
            {
                return Utilities.Spline.Position(a, b, c, d, t);
            }

            public float ApproximateLength(int samples = 20)
            {
                float newLength = 0, dT = 1f / samples;
                for (float t = 0; t <= 1; t += dT)
                {
                    var prev = Position(t);
                    var curr = Position(t + dT);
                    newLength += (prev - curr).magnitude;
                }
                return newLength;
            }
        }
    }
}