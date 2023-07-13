using UnityEngine;

namespace Species
{
    namespace Flocks
    {
        public class Boid : MonoBehaviour
        {
            FlockingSettings settings;
            // State
            [HideInInspector] public Vector3 position;
            [HideInInspector] public Vector3 forward;
            Vector3 velocity;

            // To update:
            Vector3 acceleration;
            [HideInInspector] public Vector3 avgFlockHeading;
            [HideInInspector] public Vector3 avgAvoidanceHeading;
            [HideInInspector] public Vector3 centreOfFlockmates;
            [HideInInspector] public int numPerceivedFlockmates;

            // Cached
            private Transform cachedTransform;
            [HideInInspector] public Transform target;

            void Awake()
            {
                cachedTransform = transform;
            }

            public void Initialize(FlockingSettings settings, Transform target)
            {
                this.target = target;
                this.settings = settings;

                position = cachedTransform.position;
                forward = cachedTransform.forward;
                velocity = transform.forward * (settings.minSpeed + settings.maxSpeed) / 2;
            }

            public void UpdateBoid()
            {
                Vector3 acceleration = Vector3.zero;

                if (target != null)
                {
                    Vector3 offsetToTarget = (target.position - position);
                    acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
                }

                if (numPerceivedFlockmates != 0)
                {
                    centreOfFlockmates /= numPerceivedFlockmates;

                    Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

                    var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                    var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
                    var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;

                    acceleration += alignmentForce;
                    acceleration += cohesionForce;
                    acceleration += seperationForce;
                }

                if (IsHeadingForCollision())
                {
                    Vector3 collisionAvoidDir = ObstacleRays();
                    Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
                    acceleration += collisionAvoidForce;
                }

                velocity += acceleration * Time.deltaTime;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;
                speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
                velocity = dir * speed;

                cachedTransform.position += velocity * Time.deltaTime;
                cachedTransform.forward = dir;
                position = cachedTransform.position;
                forward = dir;
            }

            private bool IsHeadingForCollision()
            {
                RaycastHit hit;
                return Physics.SphereCast(position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask);
            }

            private Vector3 ObstacleRays()
            {
                Vector3[] rayDirections = Helper.DIRECTIONS;

                for (int i = 0; i < rayDirections.Length; i++)
                {
                    Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
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
