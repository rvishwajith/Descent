using UnityEngine;
using DG.Tweening;

/* Components -> Movement -> FollowTarget
 * 
 * Description:
 * Follows a target with options for movement speed and rotation.
 * 
 */
namespace Components.Movement
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;

        [Header("Movement Settings")]
        public float maxSpeed = 5;
        public float distanceThreshold = 0.5f;

        [Header("Rotation Settings")]
        public RotationOptions rotationOptions = RotationOptions.Unlimited;
        public float maxRotationSpeed = 720;

        [Header("Gizmo Options")]
        [SerializeField] public DebugOptions debugOptions = DebugOptions.Selected;

        private Vector3 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        private Quaternion rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        private void Start()
        {
            Vector3 GenerateRandomPoint(float radius = 100)
            {
                return transform.position + Random.insideUnitSphere * radius;
            }

            var tween = transform.DOMove(transform.position + transform.forward, duration: maxSpeed)
                .SetSpeedBased(true)
                .SetLoops(-1);
            tween.OnUpdate(() =>
            {
                var targetPosition = target != null ? target.position : GenerateRandomPoint();
                tween.ChangeEndValue(targetPosition, true);
                UpdateRotation(targetPosition);
            });
            /*
            tween.OnStepComplete(() =>
            {
                targetPosition = target != null ? target.position : GenerateRandomPoint();
                Debug.Log("Movement.FollowTarget.TweenOnComplete(): Completed loop #: " + tween.CompletedLoops());
            });
            */
        }

        /*
        private void Update()
        {
            if (target == null) return;

            var offset = target.position - position;
            bool needsUpdate = offset.magnitude >= minimumTargetDistance;
            if (needsUpdate)
            {
                UpdateRotation(target.position);
                UpdateMovement();
            }
        }
        */

        private void UpdateRotation(Vector3 targetPosition)
        {
            var targetDirection = targetPosition - position;
            var desiredRotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);

            if (rotationOptions == RotationOptions.Unlimited)
                transform.LookAt(target.position, Vector3.up);
            else
                rotation = Quaternion.RotateTowards(rotation, desiredRotation, maxRotationSpeed * Time.deltaTime);
        }

        private void UpdateMovement()
        {
            float targetDist = (target.position - position).magnitude;
            position += transform.forward * Mathf.Clamp(Time.deltaTime * maxSpeed, 0, targetDist - distanceThreshold);
        }

        private void OnDrawGizmos()
        {
            if (debugOptions == DebugOptions.Always)
                DrawGizmos();
        }

        private void OnDrawGizmosSelected()
        {
            if (debugOptions == DebugOptions.Selected)
                DrawGizmos();
        }

        private void DrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 0.05f);
            Gizmos.DrawRay(position, transform.forward);
        }
    }

    public enum RotationOptions
    {
        Unlimited,
        Limited
    }
}

