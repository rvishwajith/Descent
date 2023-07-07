using UnityEngine;
using DG.Tweening;

namespace Components
{
    public class FollowPath : MonoBehaviour
    {
        public Path path = null;
        public float travelTime = 10;

        private void Start()
        {
            if (path == null) return;

            transform.DOPath(
                path: path.points,
                duration: travelTime,
                pathType: PathType.CatmullRom,
                pathMode: PathMode.Full3D,
                resolution: 10);
        }
    }
}