using System;
using DG.Tweening;
using UnityEngine;

namespace Components.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        public PlayerController controller;
        public Transform characterModel;
        public Animator animator;

        private string animIdleRef = "Idle";
        private string animSwimRef = "Swim";

        public PlayerMoveState state { get { return controller.state; } }

        public void Update()
        {
            if (state == PlayerMoveState.Transition) { return; }

            animator.SetFloat(animSwimRef, controller.relSwimSpeed);
            animator.SetFloat(animIdleRef, (1 - controller.relSwimSpeed) * 0.5f);
            animator.SetFloat("TimeScale", Mathf.Lerp(0.35f, 1f, controller.relSwimSpeed));
        }
    }
}


