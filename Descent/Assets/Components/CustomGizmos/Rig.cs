using UnityEngine;
using Utilities;

namespace Components.CustomGizmos
{
    public class DrawRig : MonoBehaviour
    {
        [Header("Skeleton")]
        public Transform root = null;

        [Header("Gizmo Settings")]
        public bool requirePrefix = true;
        public string bonePrefix = "bone";
        public bool drawBones = true;
        public bool drawHandles = true;

        private void Start()
        {
            if (root == null)
                FindRoot();
        }

        private void OnDrawGizmos()
        {
            if (drawBones && (root != null || FindRoot())) DrawHeirarchy(root);
        }

        private void DrawHeirarchy(Transform bone)
        {
            Gizmos.color = Color.gray;
            for (var i = 0; i < bone.childCount; i++)
            {
                var child = bone.GetChild(i);
                if (drawBones && IsBone(child))
                {
                    Gizmo.Bone(bone, child);
                    DrawHeirarchy(child);
                }
            }
            if (drawHandles && IsBone(bone))
                DrawHandle(bone);
        }

        private void DrawHandle(Transform parent)
        {
            var name = parent.name.Replace(bonePrefix, "").Trim();
            if (parent == root)
                name += " (Root)";
            Labels.World(name, parent.position);
            // Handles.Label(parent.position, name);
        }

        private bool IsBone(Transform child)
        {
            if (requirePrefix)
                return child.name.Contains(bonePrefix);
            else
                return true;
        }

        private bool FindRoot()
        {
            if (transform.childCount > 0 && IsBone(transform.GetChild(0)))
            {
                root = transform.GetChild(0);
                return true;
            }
            return false;
        }
    }
}