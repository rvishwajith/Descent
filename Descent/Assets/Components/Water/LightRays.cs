/* Components -> Water -> LightRays
 * 
 * Class Description:
 * Aligns the light rays to face the camera, but only on the X and Z axes. If the application is not
 * playing (editor mode), the scene camera is used instead of the main render camera.
 */

using UnityEngine;

namespace Components.Water
{
    public class LightRays : MonoBehaviour
    {
        private bool inGame = false;

        private void Start()
        {
            inGame = true;
        }

        private void Update()
        {
            if (inGame) Align();
        }

        private void OnDrawGizmos()
        {
            if (!inGame) Align();
        }

        private void Align()
        {
            if (!this.isActiveAndEnabled) return;

            Transform cameraTransform;
            if (UnityEngine.Camera.current != null)
                cameraTransform = UnityEngine.Camera.current.transform;
            else
                cameraTransform = UnityEngine.Camera.main.transform;

            var lookAtPos = cameraTransform.position;
            lookAtPos.y = transform.position.y;
            transform.LookAt(lookAtPos);
        }
    }
}
