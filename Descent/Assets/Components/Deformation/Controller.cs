using UnityEngine;
using Utilities;

namespace Components
{
    namespace Deformation
    {
        public class DeformationController : MonoBehaviour
        {
            private Mesh meshInternal = null;
            private Mesh mesh
            {
                get { return meshInternal; }
                set
                {
                    meshInternal = value;
                    vertexIn = mesh.vertices;
                    vertexOut = new Vector3[vertexIn.Length];
                    meshLength = mesh.bounds.size.z;
                }
            }

            private Vector3[] vertexIn = null;
            private Vector3[] vertexOut = null;

            public Transform[] splineTransforms = null;
            private Vector3[] splinePositions = null;
            private Curve curve = null;

            private float meshLength;
            private float splineLength;

            private void Start()
            {
                mesh = GetComponent<MeshFilter>().mesh;
            }

            private void Update()
            {
                if (mesh == null)
                    return;

                Cache();
                for (var i = 0; i < vertexIn.Length; i++)
                    vertexOut[i] = DeformVertex(vertexIn[i]);
                mesh.SetVertices(vertexOut);
            }

            private Vector3 DeformVertex(Vector3 p)
            {
                float t = Mathf.Abs(p.z / meshLength);
                float xDist = p.x, yDist = p.y;

                Vector3 pivot = Position(t);
                Vector3 xOffset = Right(t) * xDist;
                Vector3 yOffset = Up(t) * yDist;
                Vector3 offset = xOffset + yOffset;
                return pivot + offset;
            }

            private Vector3 Position(float t)
            {
                return Spline.Position(splinePositions, t);
            }

            private Vector3 Forward(float t)
            {
                var destination = Position(t - 0.01f);
                var origin = Position(t);
                return (destination - origin).normalized;
            }

            private Vector3 Right(float t)
            {
                var up = Vector3.up;
                var forward = Forward(t);
                return Vector3.Cross(forward, -up).normalized;
            }

            private Vector3 Up(float t)
            {
                var forward = Forward(t);
                var right = Vector3.right;
                return Vector3.Cross(forward, right).normalized;
            }

            private void Cache()
            {
                Vector3[] Positions()
                {
                    var newPositions = new Vector3[splineTransforms.Length];
                    for (var i = 0; i < newPositions.Length; i++)
                        newPositions[i] = splineTransforms[i].position;
                    return newPositions;
                }

                float ApproximateLength(int samples = 40)
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

                splinePositions = Positions();
                splineLength = ApproximateLength(25);
            }

            private void OnDrawGizmos()
            {
                void DrawSplineGizmos()
                {
                    void DrawSplinePoints()
                    {
                        float radius = 0.05f;
                        for (var i = 0; i < splinePositions.Length; i++)
                        {
                            Gizmos.color = new(0, 1, 1, 0.2f);
                            if (i == 0 || i == 3)
                                Gizmos.color = new(1, 1, 1, 0.1f);
                            Gizmos.DrawSphere(splinePositions[i], radius);
                        }
                    }

                    void DrawSplineSegments(float dT = 0.05f)
                    {
                        Gizmos.color = Color.yellow;
                        for (float t = dT; t <= 1 + dT; t += dT)
                        {
                            var prev = Position(t - dT);
                            var curr = Position(t);
                            Gizmos.DrawLine(prev, curr);
                        }
                    }

                    void DrawSplineForward(float dT = 0.05f)
                    {
                        Gizmos.color = Color.cyan;
                        for (float t = dT; t <= 1 + dT; t += dT)
                        {
                            var prev = Position(t - dT);
                            var curr = Position(t);
                            var length = (curr - prev).magnitude;
                            var forward = Forward(t);
                            Gizmo.Arrow(curr, forward * length);
                        }
                    }

                    void DrawSplineRight(float dT = 0.05f)
                    {
                        for (float t = 0; t <= 1 + dT; t += dT)
                        {
                            Gizmos.color = Color.Lerp(Color.red, Color.gray, t);
                            var p = Position(t);
                            Gizmo.Arrow(p, Right(t) * 0.5f);
                        }
                    }

                    void DrawSplineUp(float dT = 0.05f)
                    {
                        for (float t = 0; t <= 1 + dT; t += dT)
                        {
                            Gizmos.color = Color.Lerp(Color.green, Color.gray, t);
                            var p = Position(t);
                            Gizmo.Arrow(p, Up(t) * 0.5f);
                        }
                    }

                    Cache();
                    // DrawSplinePoints();
                    // DrawSplineSegments();
                    DrawSplineForward(0.1f);
                    DrawSplineRight(0.1f);
                    DrawSplineUp(0.1f);
                    Labels.World("Spline Length: " + splineLength, transform.position + Vector3.up);
                }
                bool SplineInitialized()
                {
                    if (splineTransforms == null || splineTransforms.Length != 4)
                        return false;
                    foreach (var curr in splineTransforms)
                    {
                        if (curr == null)
                            return false;
                    }
                    return true;
                }
                if (SplineInitialized())
                    DrawSplineGizmos();
            }
        }
    }
}