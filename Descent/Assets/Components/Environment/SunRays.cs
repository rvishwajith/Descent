using UnityEngine;
using UnityEditor;

namespace Components.Environment
{
    public class SunRays : MonoBehaviour
    {
        public WaterSettings waterSettingsAsset;

        private void OnDrawGizmos()
        {
            if (waterSettingsAsset == null)
                return;

            var position = transform.position;
            if (position.y != waterSettingsAsset.surfaceHeight)
            {
                position.y = waterSettingsAsset.surfaceHeight;
                transform.position = position;
            }
        }
    }
}
