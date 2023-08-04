using UnityEngine;

namespace Components.Camera
{
    public class TargetTracking
    {
        public Transform transform;
        public CameraController cameraController;
        public CameraSettings settings { get { return cameraController.settings; } }
        public Transform target { get { return cameraController.target; } }
        public TargetType targetType { get { return cameraController.targetType; } }
        public TrackingMode trackingMode;

        public void Update()
        {

        }
    }
}

namespace Components.Camera
{
    public enum TrackingMode
    {
        DoNothing,
        FollowPlayer,
        FollowAnimal,
        Transition
    }
}