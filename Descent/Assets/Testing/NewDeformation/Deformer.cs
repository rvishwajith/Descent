namespace ProceduralAnimation
{
    namespace Animal
    {
        using UnityEngine;

        public class MeshDeformer
        {
            private Spline spline;
            private Vector3[] originalVertices, deformedVertices;
            private float zMin, zMax;

            public MeshDeformer(Mesh mesh, Spline spline)
            {
                mesh.MarkDynamic();
                originalVertices = mesh.vertices;
                deformedVertices = new Vector3[mesh.vertexCount];

                this.spline = spline;
                zMin = Mathf.Min(mesh.bounds.min.z, mesh.bounds.max.z);
                zMax = Mathf.Max(mesh.bounds.min.z, mesh.bounds.max.z);

                Debug.Log("SplineDeformer(): Z Bounds = Min: " + zMin + " Max: " + zMax);
            }

            public Vector3[] DeformedVertices()
            {
                for (var i = 0; i < originalVertices.Length; i++)
                    deformedVertices[i] = Deform(originalVertices[i], i);
                return deformedVertices;
            }

            public Vector3 Deform(Vector3 vertex, int i)
            {
                var t = RelativeDistance(vertex.z);
                // if (i % 3000 == 0) Debug.Log("T: " + t);

                var splinePivotPoint = spline.Point(t);
                var rotatedX = spline.XTangent(t) * vertex.x;
                var rotatedY = spline.YTangent(t) * vertex.y;
                return splinePivotPoint + rotatedX + rotatedY;
            }

            public float RelativeDistance(float z)
            {
                return Mathf.Abs(z / (zMax - zMin));
            }
        }
    }
}