using System.Collections.Generic;
using UnityEngine;

namespace Kelp
{
    namespace Advanced
    {
        public class Stalk
        {
            private int integrations = 5;
            private float resetDirectionFactor = 25f, momentumDecelerationFactor = 0.9f;

            public Vector3[] points, prevPoints;
            private Vector3[] upDirections;
            private Vector3[] leafDirections;
            private float segmentLength = 0.8f;

            public Stalk(Vector3 position, float height)
            {
                float currHeight = 0;
                List<Vector3> pointsList = new();
                List<Vector3> directionsList = new();

                while (currHeight <= height)
                {
                    var x = currHeight / 2;
                    var direction = new Vector3(x, 25 * Mathf.Log10(x + 1), 0).normalized;
                    directionsList.Add(direction);

                    var point = currHeight * direction + position;
                    pointsList.Add(point);

                    currHeight += segmentLength;
                }

                upDirections = directionsList.ToArray();
                leafDirections = new Vector3[upDirections.Length];
                points = pointsList.ToArray();
                prevPoints = points;

                ComputeLeafDirections();
            }

            public void ComputeLeafDirections()
            {
                for (var i = 0; i < upDirections.Length; i++)
                {
                    leafDirections[i] = Vector3.Cross(upDirections[i], Vector3.forward).normalized;
                }
                // leafDirections = Vector3.Cross(upDirections, Vector3.right);
            }

            public void Update(float dT, Collider[] colliders)
            {
                ApplyPointForces(dT);
                for (var i = 0; i < integrations; i++)
                    IntegrateSegments();
                CollisionConstraints(colliders);

                for (var i = 0; i < points.Length; i++)
                    prevPoints[i] = points[i];
            }

            private void ApplyPointForces(float dT)
            {
                for (var i = 0; i < points.Length; i++)
                {
                    if (i == 0) continue;

                    // Force 1: Momentum
                    // Applies the same movement from the previous frame with decreasing
                    // strength over time (based on deceleration factor).
                    var momentum = points[i] - prevPoints[i];
                    var deceleration = dT / momentumDecelerationFactor;
                    points[i] += momentum * deceleration;

                    // Force 2: Reset Direction Force (Like gravity, but in any direction)
                    // Applies a constant (over dT^2) force towards the initial direction
                    // of the leaf.
                    var resetForce = upDirections[i] * resetDirectionFactor * (dT * dT);
                    points[i] += resetForce;
                }
            }

            private void IntegrateSegments()
            {
                for (var i = 1; i < points.Length; i++)
                {
                    Vector3 a = points[i - 1], b = points[i],
                        center = (a + b) / 2,
                        dir = (a - b).normalized;

                    // The root point (0) is locked to the kelp stalk.
                    if (i - 1 != 0)
                        points[i - 1] = center + dir * segmentLength / 2;
                    if (i != 0)
                        points[i] = center - dir * segmentLength / 2;
                }
            }

            private void CollisionConstraints(Collider[] colliders)
            {
                // Collision Constraints:
                // If the point is within a collider, move it to the closest
                // position on the surface of the collider.
                foreach (var collider in colliders)
                {
                    if (collider.GetType() == typeof(SphereCollider))
                    {
                        var radius = ((SphereCollider)collider).radius * collider.transform.lossyScale.x;
                        for (var i = 1; i < points.Length; i++)
                        {
                            var diff = points[i] - collider.transform.position;
                            if (diff.magnitude < radius)
                            {
                                points[i] = collider.transform.position + diff.normalized * radius;
                            }
                        }
                    }
                }
            }

            public void DrawGizmos()
            {
                var radius = 0.04f;
                Gizmos.color = Color.cyan;
                for (var i = 1; i < points.Length; i++)
                {
                    Gizmos.DrawLine(points[i - 1], points[i]);
                }

                Gizmos.color = Color.cyan;
                radius = 0.04f;
                for (var i = 0; i < points.Length; i++)
                {
                    Gizmos.DrawCube(points[i], Vector3.one * radius);
                }

                /*
                Gizmos.color = Color.green;
                for (var i = 1; i < points.Length; i++)
                {
                    var origin = Vector3.Lerp(points[i - 1], points[i], 0.5f);
                    var direction = Vector3.Cross(upDirections[i], Vector3.forward).normalized;
                    Gizmos.DrawRay(origin, direction * 1.5f);
                }
                */
            }

            public Vector3 PointOnStalk(int index, float t)
            {
                return Vector3.Lerp(points[index], points[index + 1], t);
            }

            public Vector3 LeafDirection(int index, float t)
            {
                return Vector3.Cross(Vector3.Lerp(upDirections[index], upDirections[index + 1], t), Vector3.forward).normalized;
            }
        }
    }
}