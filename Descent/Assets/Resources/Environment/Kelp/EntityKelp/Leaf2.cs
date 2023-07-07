namespace Kelp
{
    using UnityEngine;

    namespace Advanced
    {
        public class Leaf2
        {
            private int stalkIndex;
            private float stalkIndexOffset;

            private int numPoints = 4;
            private Vector3[] points, prevPoints;
            private Stalk stalk;

            private float segmentLength = 0.25f;
            private int integrations = 3;

            private Vector3 flowDirection;
            private float flowDirectionFactor = 25f, momentumDecelerationFactor = 1f;

            public Leaf2(Stalk stalk, int index, float offset)
            {
                points = new Vector3[numPoints];
                prevPoints = new Vector3[numPoints];

                this.stalk = stalk;
                this.stalkIndex = index;
                this.stalkIndexOffset = offset;

                var origin = stalk.PointOnStalk(index, offset);
                flowDirection = stalk.LeafDirection(index, offset);

                for (var i = 0; i < points.Length; i++)
                {
                    points[i] = origin + flowDirection * i * segmentLength;
                    prevPoints[i] = points[i];
                }
            }

            public void Update(float dT, Collider[] colliders)
            {
                points[0] = stalk.PointOnStalk(stalkIndex, stalkIndexOffset);
                flowDirection = stalk.LeafDirection(stalkIndex, stalkIndexOffset);

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
                    var resetForce = flowDirection * flowDirectionFactor * (dT * dT);
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
                // Gizmos.color = Color.green;
                // Gizmos.DrawWireCube(points[0], Vector3.one * radius);

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
