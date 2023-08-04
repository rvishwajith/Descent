using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Components.Rendering
{
    public class CameraFrustumShaderPass : MonoBehaviour
    {
        [SerializeField] private bool debugValues = false;
        [SerializeField] private Vector2 debugPoint = Vector2.one * 0.5f;

        private UnityEngine.Camera gameCamera;
        private string leftBottomRef = "_NearPlaneLeftBottom",
            leftTopRef = "_NearPlaneLeftTop",
            rightBottomRef = "_NearPlaneRightBottom",
            rightTopRef = "_NearPlaneRightTop";

        public void Start()
        {
            Init();
        }

        public void Init()
        {
            gameCamera = GetComponentInParent<UnityEngine.Camera>();
        }

        public void Update()
        {
            PassNearClipData();
        }

        public void PassNearClipData()
        {
            float nearClipZ = gameCamera.nearClipPlane;
            Shader.SetGlobalVector(leftBottomRef, gameCamera.ViewportToWorldPoint(new(0, 0, nearClipZ)));
            Shader.SetGlobalVector(leftTopRef, gameCamera.ViewportToWorldPoint(new(0, 1, nearClipZ)));
            Shader.SetGlobalVector(rightBottomRef, gameCamera.ViewportToWorldPoint(new(1, 0, nearClipZ)));
            Shader.SetGlobalVector(rightTopRef, gameCamera.ViewportToWorldPoint(new(1, 1, nearClipZ)));
        }

        private void OnDrawGizmos()
        {
            if (debugValues == false)
                return;
            else if (gameCamera == null)
            {
                Init();
                return;
            }
            gameCamera = UnityEngine.Camera.current;

            var points = new Vector3[] {
                Shader.GetGlobalVector(leftBottomRef),
                Shader.GetGlobalVector(leftTopRef),
                Shader.GetGlobalVector(rightBottomRef),
                Shader.GetGlobalVector(rightTopRef) };

            Gizmos.color = Color.green;
            foreach (var point in points)
                Gizmos.DrawSphere(point, 0.1f);
            Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(gameCamera.ViewportToWorldPoint(new(debugPoint.x, debugPoint.y, gameCamera.nearClipPlane)), 0.1f);
            // Debug.Log(Utilities.Format.Array(points, 2));
        }
    }
}