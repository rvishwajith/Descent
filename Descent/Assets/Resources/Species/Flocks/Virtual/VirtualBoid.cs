using UnityEditor;
using UnityEngine;
using Components;

namespace Species
{
    namespace Flocks
    {
        public class VirtualBoid
        {
            public FlockingSettings settings;

            // Cached
            public VirtualTransform transform;
            public Transform target;

            public Vector3 position;
            // Glitch: forward is backwards?
            public Vector3 forward;
            public Vector3 velocity;

            // To update:
            public Vector3 acceleration;
            public Vector3 avgFlockHeading;
            public Vector3 avgAvoidanceHeading;
            public Vector3 centreOfFlockmates;
            public int numPerceivedFlockmates;

            public VirtualBoid(VirtualTransform vT, FlockingSettings settings)
            {
                this.transform = vT;
                this.settings = settings;

                position = vT.position;
                forward = vT.forward;
                velocity = vT.forward * (settings.minSpeed + settings.maxSpeed) / 2;
            }

            public void UpdateForces()
            {
                acceleration = Vector3.zero;
                if (target != null)
                {
                    Vector3 offsetToTarget = (target.position - position);
                    acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
                }
                if (numPerceivedFlockmates != 0)
                {
                    centreOfFlockmates /= numPerceivedFlockmates;

                    Vector3 offsetToPercievedCenter = (centreOfFlockmates - position);
                    var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                    var cohesionForce = SteerTowards(offsetToPercievedCenter) * settings.cohesionWeight;
                    var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;
                    acceleration += alignmentForce + cohesionForce + seperationForce;
                }
            }

            public void Move(float dT)
            {
                velocity += acceleration * dT;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;
                speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
                velocity = dir * speed;

                transform.position += velocity * dT;
                transform.forward = dir;
                position = transform.position;
                forward = dir;
            }

            public void UpdateCollisionAvoidance()
            {
                if (IsHeadingForCollision())
                {
                    Vector3 collisionAvoidDir = ObstacleRays();
                    Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
                    acceleration += collisionAvoidForce;
                }
            }

            private bool IsHeadingForCollision()
            {
                return Physics.SphereCast(
                    position,
                    settings.boundsRadius,
                    forward, out _,
                    settings.collisionAvoidDst,
                    settings.obstacleMask);
            }

            private Vector3 ObstacleRays()
            {
                Vector3[] rayDirections = Helper.DIRECTIONS;

                for (int i = 0; i < rayDirections.Length; i++)
                {
                    Vector3 dir = transform.TransformDirection(rayDirections[i]);
                    Ray ray = new Ray(position, dir);
                    if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
                        return dir;
                }
                return forward;
            }

            private Vector3 SteerTowards(Vector3 vector)
            {
                Vector3 v = vector.normalized * settings.maxSpeed - velocity;
                return Vector3.ClampMagnitude(v, settings.maxSteerForce);
            }
        }
    }
}
