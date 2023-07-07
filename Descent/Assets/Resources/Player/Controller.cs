using UnityEngine;
using DG.Tweening;
using Utilities;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        public PlayerSettings settings;

        [HideInInspector] public float speed = 0;
        [HideInInspector] public float surfaceHeight = 50;

        private Movement movement;
        private Steering steering;
        private Transform rig = null;

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Vector3 Forward
        {
            set { transform.forward = value.normalized; }
            get { return transform.forward; }
        }

        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        private Vector3 eulerAngles = Vector3.zero;
        public Vector3 EulerAngles
        {
            get { return eulerAngles; }
            set
            {
                eulerAngles = value;
                eulerAngles.x = Mathf.Clamp(value.x, settings.diveMin, settings.diveMax);
                eulerAngles.z = 0;
                Rotation = Quaternion.Euler(eulerAngles);
            }
        }

        private int state;
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private void Start()
        {
            movement = new(this);
            steering = new(this);
            rig = transform.Find("Rig");
        }

        private void Update()
        {
            Move();
            if (speed > 0.03f)
                Steer();
            StateManagement();
        }

        private void StateManagement()
        {
            if (State == 0 && Input.GetKeyDown(KeyCode.LeftShift))
            {
                State = -1;
                rig.DOLocalRotate(Vector3.right * 90, 0.4f, RotateMode.Fast)
                    .SetDelay(0.1f)
                    .OnComplete(() =>
                    {
                        State = 2;
                    });
            }
            else if (state == 2 && movement.RelativeSpeed() <= 0.03f)
            {
                var newAngles = EulerAngles;
                newAngles.x = 0;
                State = -1;
                transform.DOLocalRotate(newAngles, 0.5f, RotateMode.Fast).SetDelay(0f);
                rig.DOLocalRotate(Vector3.zero, 0.7f, RotateMode.Fast)
                    .SetDelay(0.2f)
                    .OnComplete(() => { State = 0; })
                    .SetDelay(0.15f);
            }
        }

        private void Move()
        {
            speed = movement.RelativeSpeed() * settings.speedMax * Time.deltaTime;
            Position += Forward * speed;
        }

        private void Steer()
        {
            Vector3 multiplier = new(settings.diveMultiplier, settings.turnMultiplier);
            Vector3 steerAmount = Vector3.Scale(steering.Rotation, multiplier) * Time.deltaTime;
            EulerAngles += steerAmount;
        }

        private void OnDrawGizmos()
        {
            Utilities.Labels.Screen("Speed: " + speed + "\nState: " + State, new(0.1f, 0.1f));
        }
    }
}