using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        [HideInInspector] public float speed = 0;
        [HideInInspector] public float maxSpeed = 8;

        [HideInInspector] public float surfaceHeight = 50;
        [HideInInspector] public float minDiveAngle = -65;
        [HideInInspector] public float maxDiveAngle = 80;
        [HideInInspector] public float turnMultiplier = 75;
        [HideInInspector] public float diveMultiplier = 45;

        private Movement movement = new();
        private Steering steering = new();

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        private Vector3 angles = Vector3.zero;
        public Vector3 Angles
        {
            get { return angles; }
            set
            {
                angles = value;
                angles.x = Mathf.Clamp(value.x, minDiveAngle, maxDiveAngle);
            }
        }

        private void Update()
        {
            Steering();
            Movement();
        }

        private void Movement()
        {
            speed = movement.RelativeSpeed() * maxSpeed;
            Position += transform.forward * speed * Time.deltaTime;
        }

        private void Steering()
        {
            var multiplier = new Vector3(diveMultiplier, turnMultiplier, 0);
            var rotation = Vector3.Scale(steering.Rotation, multiplier);

            Angles += rotation * Time.deltaTime;
            Rotation = Quaternion.Euler(Angles.x, Angles.y, 0);
        }
    }
}