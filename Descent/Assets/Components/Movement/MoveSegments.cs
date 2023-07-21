using System;
using UnityEditor;
using UnityEngine;

namespace Components
{
    public class MoveSegments : MonoBehaviour
    {
        [SerializeField] private Segment[] segments;

        [Header("Debug Options")]
        public DebugOptions gizmoOptions = DebugOptions.Selected;
        public DebugOptions labelOptions = DebugOptions.Selected;

        // [Tooltip("If enabled, the segment detaches itself from its current parent. A new empty object is added to the heirarchy and replaces the previous parent.")]
        private bool detachFromParent = true;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i].target == null)
                {
                    segments[i].detachFromParent = detachFromParent;
                    var target = i > 0 ? segments[i - 1].transform : this.transform;
                    segments[i].Init(target);
                }
            }
        }

        private void Update()
        {
            foreach (var segment in segments)
                segment.Update();
        }

        private void OnDrawGizmos()
        {
            if (segments == null) return;

            if (gizmoOptions == DebugOptions.Always)
            {
                Gizmos.color = Color.yellow;
                foreach (var segment in segments)
                    segment.DrawGizmos();
            }
            if (labelOptions == DebugOptions.Always)
            {
                foreach (var segment in segments)
                    segment.DrawLabels();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (segments == null) return;

            if (gizmoOptions == DebugOptions.Selected)
            {
                Gizmos.color = Color.yellow;
                foreach (var segment in segments)
                    segment.DrawGizmos();
            }
            if (labelOptions == DebugOptions.Selected)
            {
                foreach (var segment in segments)
                    segment.DrawLabels();
            }
        }
    }
}
