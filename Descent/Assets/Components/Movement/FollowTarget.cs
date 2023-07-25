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

        [Header("Speed & Rotation Settings")]
        public float maxSpeed = 5;
        public bool limitRotation = true;
        public float maxRotationRate = 60;

        [Header("Gizmo Options")]
        public DebugOptions debugOptions = DebugOptions.Selected;

        private float rotationSpeed { get { return limitRotation ? maxRotationRate : 10000; } }

        private void Update()
        {
            if (target == null)
            {
                transform.position += transform.forward * (maxSpeed / 2) * Time.deltaTime;
                return;
            }

            var distance = Vector3.Distance(target.position, transform.position);
            if (distance <= 0.1f)
            {
                transform.position += transform.forward * (maxSpeed / 2) * Time.deltaTime;
                return;
            }

            Quaternion initialRotation = transform.rotation;
            transform.LookAt(target.position, Vector3.up);
            Quaternion desiredRotation = transform.rotation;

            transform.rotation = Quaternion.RotateTowards(initialRotation, desiredRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * Mathf.Min(maxSpeed * Time.deltaTime, distance);
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
            Gizmos.DrawSphere(transform.position, 0.05f);
            Gizmos.DrawRay(transform.position, transform.forward);
        }
    }

    public enum RotationOptions
    {
        Unlimited,
        Limited
    }
}

