using UnityEngine;

namespace Components.Water
{
    public class MatchWaterSurfaceHeight : MonoBehaviour
    {
        public Components.Water.WaterSettings waterSettingsAsset;

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