using UnityEngine;

namespace Components.Deformation
{
    public class Controller : MonoBehaviour
    {
        public Transform[] points;
        [Range(0.0f, 1.0f)] public float testPoint = 0;

        private CurveController curveController;
        private MeshController meshController;

        private void Start()
        {
            InitCurveController();
            InitMeshController();
        }

        private void InitCurveController()
        {
            if (points != null && points.Length < 4)
                curveController = new(points);
        }

        private void InitMeshController()
        {
            meshController = new(this, true);
        }

        private void Update()
        {
            /*
            if (!initialized)
            {
                Init();
                return;
            }
            curve.Cache();
            */
        }

        private void OnDrawGizmos()
        {
            if (curveController == null || !curveController.initialized)
                InitCurveController();
            else
            {
                Debug.Log("Drawing curve gizmos.");
                curveController.Cache();
                curveController.DrawGizmos(testPoint);
            }

            if (Application.isPlaying)
            {
                if (meshController == null || !meshController.initialized)
                    InitMeshController();
                else
                    meshController.DrawGizmos();
            }
        }
    }
}