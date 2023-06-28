using UnityEngine;

namespace Utilities
{
    namespace Gizmo
    {
        public static class Draw
        {
            public static void Bone(Transform root, Transform child)
            {
                Gizmos.DrawWireSphere(root.position, 0.03f);
                Gizmos.DrawWireSphere(child.position, 0.03f);

                float rootEndProportion = 0.3f;
                float width = (root.position - child.position).magnitude * 0.2f;
                var rootEnd = Vector3.Lerp(root.position, child.position, rootEndProportion);
                Draw.Pyramid(root.position, rootEnd, root.up, root.right, width);
                Draw.Pyramid(child.position, rootEnd, root.up, root.right, width);
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
        }
    }
}
