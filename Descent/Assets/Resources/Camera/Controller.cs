using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicCamera
{
    public class Controller : MonoBehaviour
    {
        new private Transform camera;
        private TargetTracking tracker;

        private Transform target;
        public Transform Target
        {
            get { return target; }
            set
            {
                target = value;
                Debug.Log("DynamicCamera.TargetChanged(): Target = " + Target.name);
                offset = CameraPosition - TargetPosition;
            }
        }
        public Vector3 TargetPosition
        {
            get { return Target.position; }
            set { Target.position = value; }
        }
        public Vector3 CameraPosition
        {
            get { return camera.position; }
            set { camera.position = value; }
        }
        private Vector3 offset;

        private bool shaking = false;
        private float shakeStrength = 0;
        public float ShakeStrength
        {
            get { return shakeStrength; }
            set
            {
                shakeStrength = value;
                shaking = value != 0;
            }
        }
        public float FOV
        {
            get { return Camera.main.fieldOfView; }
            set { Camera.main.fieldOfView = value; }
        }

        private void Start()
        {
            if (camera == null)
                camera = Camera.main.transform;
            Target = GameObject.Find("Player").transform;
            tracker = new(this);
        }

        private void Update()
        {
            tracker.Update();
            CameraPosition = Vector3.Lerp(CameraPosition, offset + tracker.TrailEnd, Time.deltaTime * 3);
            camera.LookAt(TargetPosition);

            float minFOV = 50, maxFOV = 85;
            float maxVelocity = 10f;
            float relVelocity = Mathf.Clamp(tracker.TargetVelocity / maxVelocity, 0, 1);

            float desiredVelocity = Mathf.Lerp(minFOV, maxFOV, relVelocity);
            FOV = Mathf.SmoothStep(FOV, desiredVelocity, Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (tracker != null)
                tracker.DrawGizmos();
        }
    }

    class TargetTracking
    {
        private Controller controller;
        private List<Vector3> history = new();
        public float minMoveAmount = 0.1f;

        public float maxTravelDistance = 13f;
        private float distanceTravelled = 0;

        public Vector3 TrailEnd
        {
            get { return history[0]; }
        }

        private Vector3 PreviousTargetPosition
        {
            get { return history[history.Count - 1]; }
            set
            {
                if (history.Count == 0)
                    history.Add(value);
                else
                {
                    var moveAmount = (value - PreviousTargetPosition).magnitude;
                    if (moveAmount > minMoveAmount)
                    {
                        history.Add(value);
                        distanceTravelled += moveAmount;

                        while (distanceTravelled > maxTravelDistance && history.Count > 2)
                        {
                            var dist = (history[1] - history[0]).magnitude;
                            history.RemoveAt(0);
                            distanceTravelled -= dist;
                        }
                    }
                }
            }
        }

        public float TargetVelocity
        {
            get { return (TargetPosition - PreviousTargetPosition).magnitude / Time.deltaTime; }
        }

        public Vector3 TargetPosition
        {
            get { return controller.TargetPosition; }
        }

        public TargetTracking(Controller c)
        {
            this.controller = c;
            this.PreviousTargetPosition = TargetPosition;
        }

        public void Update()
        {
            if (PreviousTargetPosition != TargetPosition)
            {
                this.PreviousTargetPosition = TargetPosition;
                // Debug.Log("Target Moved!");
                // Debug.Log(history.Count);
            }
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            for (var i = 1; i < history.Count; i++)
            {
                var prev = history[i - 1];
                var next = history[i];
                Gizmos.DrawLine(prev, next);
            }
        }
    }
}