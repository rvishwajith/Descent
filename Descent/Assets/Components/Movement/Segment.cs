using System;
using UnityEngine;
using Utilities;

namespace Components
{
    [Serializable]
    public class Segment
    {
        [SerializeField] public Transform transform;
        [HideInInspector] public Transform target;
        [HideInInspector] public bool detachFromParent;

        private TargetState targetState;
        private Vector3 prevTargetPos;
        private float distance;

        public void Init(Transform target)
        {
            this.target = target;
            prevTargetPos = target.position;
            distance = (target.position - transform.position).magnitude;

            if (detachFromParent)
            {
                var child = this.transform;
                var childPosition = child.position;
                var parentPosition = child.position;

                var parent = new GameObject(child.name).transform;
                parentPosition.y = 0;
                parent.position = parentPosition;

                // childPosition.x = 0;
                // childPosition.z = 0;
                child.parent = parent;
                // childTransform.localPosition = childPosition;

                parent.LookAt(target.position);
                this.transform = parent;
            }
        }

        public void Update()
        {
            targetState = target.position != prevTargetPos ? TargetState.Moving : TargetState.Stationary;
            if (targetState == TargetState.Moving)
            {
                MoveSegment();
            }
        }

        private void MoveSegment()
        {
            var offset = target.position - transform.position;
            var newPosition = target.position - offset.normalized * distance;

            transform.position = newPosition;
            transform.LookAt(target.position);

            prevTargetPos = target.position;
        }

        public void DrawGizmos()
        {
            if (transform == null)
                return;
            Gizmos.DrawSphere(transform.position, 0.05f);

            if (target == null)
                return;
            Gizmo.Arrow(transform.position, target.position - transform.position);
            if (transform.childCount > 0)
                Gizmos.DrawSphere(transform.GetChild(0).position, 0.05f);
        }

        public void DrawLabels()
        {
            if (target != null)
                Labels.World(targetState + "", transform.position + Vector3.up / 3);
        }
    }

    public enum TargetState
    {
        Stationary,
        Moving
    }
}