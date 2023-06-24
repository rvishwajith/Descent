namespace Kelp
{
    using UnityEngine;

    public class LeafDeformation : MonoBehaviour
    {
        public Transform[] splineTransforms;
        private LeafSpline spline;
        private LeafDeformer deformer;
        private bool initialized = false;

        void Start()
        {
            spline = new(splineTransforms);
            deformer = new(spline, transform, GetComponent<MeshFilter>().mesh);
            initialized = true;
        }

        void Update()
        {
            deformer.UpdateMesh();
        }

        private void OnDrawGizmos()
        {
            if (initialized && splineTransforms.Length >= 4)
            {
                if (deformer != null)
                    deformer.DrawGizmos();
                if (spline != null)
                    spline.DrawGizmos();
            }
        }
    }
}