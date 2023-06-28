using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicCamera
{
    public class Controller : MonoBehaviour
    {
        private Transform target;
        public Transform Target
        {
            get { return target; }
            set
            {
                target = value;
                TargetChanged();
            }
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

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

        }

        public void Update()
        {

        }

        public void TargetChanged()
        {
            Debug.Log("DynamicCamera.TargetChanged(): New target - " + Target.name);
        }
    }
}
