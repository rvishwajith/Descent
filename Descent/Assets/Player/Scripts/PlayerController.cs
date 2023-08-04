using UnityEngine;
using DG.Tweening;
using Utilities;

namespace Components.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerSettings settings;
        [HideInInspector] public PlayerMoveState state = PlayerMoveState.Idle;
        [HideInInspector] public float relSwimSpeed = 0;

        private Vector2 turnSpeedLastFrame = Vector2.zero;

        private void Update()
        {
            Movement();
            Rotation();

            if (Input.GetKey(KeyCode.Backspace))
            {
                relSwimSpeed = 0;
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
                state = PlayerMoveState.Idle;
            }
        }

        private void Movement()
        {
            state = PlayerMoveState.Swimming;

            if (Input.GetKey(settings.swimKey))
            {
                relSwimSpeed += Time.deltaTime * settings.movement.relSwimAcceleration;
            }
            else
            {
                relSwimSpeed -= Time.deltaTime * 1f;
            }
            relSwimSpeed = Mathf.Clamp(relSwimSpeed, 0, 1);

            var actualMoveSpeed = settings.movement.swimSpeedCurve.Evaluate(relSwimSpeed) * settings.movement.swimSpeedMax;
            transform.position += transform.forward * actualMoveSpeed * Time.deltaTime;
        }

        private void Rotation()
        {
            var steeringDir = Vector2.zero;

            if (Input.GetKey(settings.steerRightKey))
                steeringDir.y = 1;
            else if (Input.GetKey(settings.steerLeftKey))
                steeringDir.y = -1;

            if (Input.GetKey(settings.steerUpKey))
                steeringDir.x = -1;
            else if (Input.GetKey(settings.steerDownKey))
                steeringDir.x = 1;

            steeringDir = Vector2.Scale(steeringDir, settings.steeringSpeeds).normalized;
            var steeringMultiplier = settings.movement.steerSpeedMultiplierCurve.Evaluate(relSwimSpeed);

            var actualTurnSpeed = steeringMultiplier * Vector2.Scale(steeringDir, settings.steeringSpeeds);

            // LERPING: MAY OR MAY NOT WORK
            actualTurnSpeed = Vector2.Lerp(turnSpeedLastFrame, actualTurnSpeed, Time.deltaTime);

            transform.Rotate(Vector3.right, Time.deltaTime * actualTurnSpeed.x, Space.World);
            transform.Rotate(Vector3.up, Time.deltaTime * actualTurnSpeed.y, Space.World);

            turnSpeedLastFrame = actualTurnSpeed;
        }

        private void OnGUI()
        {
            var content = "State: " + state +
                "\nRelative Speed: " + Format.Float(relSwimSpeed, 2);

            Draw.Label(new(20, 20, 320, 100), content);
        }
    }
}

namespace Components.Player
{
    public enum PlayerMoveState
    {
        Idle,
        Swimming,
        Transition,
        Boosting
    }
}