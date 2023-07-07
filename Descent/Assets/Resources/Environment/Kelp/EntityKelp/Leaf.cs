namespace Kelp
{
    using UnityEngine;

    namespace Advanced
    {
        public class Leaf
        {
            private int numPoints = 4;
            private Vector3[] points, prevPoints;

            // Integration
            private float segmentLength;
            private int integrations = 3;

            // Forces
            private Vector3 resetDirection;
            private float resetDirectionFactor = 30f, momentumDecelerationFactor = 1f;

            public Leaf(Vector3 origin, Vector3 dir, float length)
            {
                points = new Vector3[numPoints];
                prevPoints = new Vector3[numPoints];
                segmentLength = length / (numPoints - 1);

                resetDirection = dir.normalized;
                for (var i = 0; i < points.Length; i++)
                {
                    var dist = length * i;
                    points[i] = origin + resetDirection * dist;
                    prevPoints[i] = points[i];
                }
            }

            public void Update(float dT, Collider[] colliders)
            {
                ApplyPointForces(dT);
                for (var i = 0; i < integrations; i++)
                    IntegrateSegments();
                ApplyCollisionConstraints(colliders);

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
                    var resetForce = resetDirection * resetDirectionFactor * (dT * dT);
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

                    // Points at 0 are locked to the kelp stalk.
                    if (i - 1 != 0)
                        points[i - 1] = center + dir * segmentLength / 2;
                    if (i != 0)
                        points[i] = center - dir * segmentLength / 2;
                }
            }

            void ApplyCollisionConstraints(Collider[] colliders)
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
                var radius = 0.02f;
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(points[0], Vector3.one * radius);

                radius = 0.04f;
                Gizmos.color = Color.white;
                for (var i = 1; i < points.Length; i++)
                    Gizmos.DrawCube(points[i], Vector3.one * radius);

                Gizmos.color = Color.gray;
                for (var i = 1; i < points.Length; i++)
                    Gizmos.DrawLine(points[i - 1], points[i]);
            }
        }
    }
}
