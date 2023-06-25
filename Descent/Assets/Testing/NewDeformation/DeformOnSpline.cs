using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    namespace Animal
    {
        public class DeformOnSpline : MonoBehaviour
        {
            [Header("Spline Points")]
            public Transform[] points;
            private Spline spline;

            private Mesh mesh;
            private MeshDeformer deformer;

            void Start()
            {
                spline = new(transform, points);
                mesh = GetComponent<MeshFilter>().mesh;
                deformer = new(mesh, spline);
            }

            public void FixedUpdate()
            {
                spline.Update();
                mesh.vertices = deformer.DeformedVertices();
                mesh.RecalculateNormals();
                // Debug.Log("Length: " + spline.Length());
            }

            private void OnDrawGizmos()
            {
                if (spline == null)
                    spline = new(transform, points);
                spline.DrawGizmos();
            }
        }
    }
}
