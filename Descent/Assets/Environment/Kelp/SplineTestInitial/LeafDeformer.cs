namespace Kelp
{
    using UnityEngine;

    public class LeafDeformer
    {
        private LeafSpline spline;
        private Transform transform;

        private Mesh mesh;
        private Vector3[] originalVertices, vertices;
        private float min, max;

        public LeafDeformer(LeafSpline spline, Transform transform, Mesh mesh)
        {
            mesh.MarkDynamic();
            min = mesh.bounds.min.x;
            max = mesh.bounds.max.x;
            originalVertices = mesh.vertices;
            vertices = new Vector3[mesh.vertexCount];

            this.spline = spline;
            this.transform = transform;
            this.mesh = mesh;
        }

        public void UpdateMesh()
        {
            spline.UpdatePoints();
            for (var i = 0; i < originalVertices.Length; i++)
            {
                vertices[i] = UpdateVertex(originalVertices[i]);
            }
            mesh.SetVertices(vertices);
        }

        public Vector3 UpdateVertex(Vector3 pos)
        {
            var x = pos.x;
            var relativeWidth = Mathf.InverseLerp(min, max, x);

            var newZ = spline.Position(relativeWidth).z;
            pos.z = newZ;

            return pos;
        }

        public Vector3 LocalToWorldPoint(Vector3 localPoint)
        {
            return transform.TransformPoint(localPoint);
        }

        public Vector3 WorldToLocalPoint(Vector3 worldPoint)
        {
            return transform.InverseTransformPoint(worldPoint);
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.cyan;
            // Gizmos.DrawWireSphere(LocalToWorldPoint(new(min, 0, 0)), 0.075f);
            // Gizmos.DrawWireSphere(LocalToWorldPoint(new(max, 0, 0)), 0.075f);

            Gizmos.DrawWireMesh(mesh);
        }
    }
}