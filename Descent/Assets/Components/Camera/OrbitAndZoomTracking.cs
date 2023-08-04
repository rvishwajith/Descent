using UnityEngine;

namespace Components.Camera
{
    public class OrbitAndZoomController
    {
        public CameraController controller;
        public Transform transform;
        public OrbitGestureMode orbitGestureModes = OrbitGestureMode.Any;

        public Transform target { get { return controller.target; } }
        public CameraSettings settings { get { return controller.settings; } }

        public void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                var input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                DetectedOrbitGesture(input);
                return;
            }
            if (Input.mouseScrollDelta.y > 0)
            {
                var input = Input.mouseScrollDelta;
                DetectedZoomGesture(input.y);
            }
        }

        public void DetectedOrbitGesture(Vector2 input)
        {
            Debug.Log("DetectedOrbitGesture() Input " + Utilities.Format.Vector(input, 2));
        }

        public void DetectedZoomGesture(float input)
        {
            Debug.Log("DetectedZoomGesture() Input " + Utilities.Format.Float(input, 1));
        }
    }

    public enum OrbitGestureMode
    {
        Any,
        MousePan,
        TouchDrag
    }
}

