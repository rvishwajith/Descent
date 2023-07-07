using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class DoorTrigger : MonoBehaviour
    {
        public Door door;
        public Transform orb;

        public float openSequenceDelay = 1.5f;

        private float yCenter = 0f,
            yOffsetDist = 0.3f,
            yOffsetSpeed = 1f;
        private float zRotationSpeed = 90f,
            xRotationSpeed = 30f,
            yRotationSpeed = 15f;

        void Update()
        {
            orb.localPosition = Vector3.up * (yCenter + Mathf.Sin(Time.time * yOffsetSpeed) * yOffsetDist);
            orb.localEulerAngles = new Vector3(xRotationSpeed, yRotationSpeed, zRotationSpeed) * Time.time;
        }

        private void OnTriggerEnter(Collider other)
        {
            Invoke("Interact", 1.5f);
        }

        public void Interact()
        {
            door.State = 1;
        }
    }
}
