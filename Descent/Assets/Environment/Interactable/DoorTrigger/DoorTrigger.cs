using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class DoorTrigger : MonoBehaviour
    {
        public Door door;
        public Transform orb;

        private float zRotationSpeed = 90f, // Degrees per second.
            xRotationSpeed = 30f,
            yRotationSpeed = 15f;

        private float yCenter = 0f, yOffsetDist = 0.3f, yOffsetSpeed = 1f;

        void Update()
        {
            orb.localPosition = Vector3.up * (yCenter + Mathf.Sin(Time.time * yOffsetSpeed) * yOffsetDist);
            orb.localEulerAngles = new Vector3(xRotationSpeed, yRotationSpeed, zRotationSpeed) * Time.time;
        }

        private void OnTriggerEnter(Collider other)
        {
            Interact();
        }

        public void Interact()
        {
            door.Open(1.5f);
        }
    }
}
