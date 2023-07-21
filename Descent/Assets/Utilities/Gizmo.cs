using UnityEngine;

namespace Utilities
{
    public static class Gizmo
    {
        public static Color color
        {
            get { return Gizmos.color; }
            set { Gizmos.color = value; }
        }

        public static void Bone(Transform root, Transform child)
        {
            Gizmos.DrawWireSphere(root.position, 0.03f);
            Gizmos.DrawWireSphere(child.position, 0.03f);

            float rootEndProportion = 0.3f;
            float width = (root.position - child.position).magnitude * 0.2f;
            var rootEnd = Vector3.Lerp(root.position, child.position, rootEndProportion);
            Gizmo.Pyramid(root.position, rootEnd, root.up, root.right, width);
            Gizmo.Pyramid(child.position, rootEnd, root.up, root.right, width);
        }

        public static void Pyramid(Vector3 vertex, Vector3 baseCenter, Vector3 up, Vector3 right, float width = -1)
        {
            float height = (vertex - baseCenter).magnitude;
            if (width == -1)
                width = height * 0.2f;
            width /= 2;

            Vector3 dir1 = baseCenter + (up + right).normalized * width,
                dir2 = baseCenter + (up - right).normalized * width,
                dir3 = baseCenter + (-up + right).normalized * width,
                dir4 = baseCenter + (-up - right).normalized * width;
            Gizmos.DrawLine(vertex, dir1);
            Gizmos.DrawLine(vertex, dir2);
            Gizmos.DrawLine(vertex, dir3);
            Gizmos.DrawLine(vertex, dir4);

            Gizmos.DrawLine(dir1, dir2);
            Gizmos.DrawLine(dir1, dir3);
            Gizmos.DrawLine(dir2, dir4);
            Gizmos.DrawLine(dir3, dir4);
        }

        public static void Arrow(Vector3 pos, Vector3 dir, float headLength = 0.1f, float headAngle = 45f)
        {
            if (dir.magnitude > 0)
            {
                Gizmos.DrawRay(pos, dir);
                Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 + headAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 - headAngle, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(pos + dir, right * headLength);
                Gizmos.DrawRay(pos + dir, left * headLength);
            }
        }
    }
}
