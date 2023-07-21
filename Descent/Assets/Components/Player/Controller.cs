using UnityEngine;

namespace Components.Player
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] public PlayerSettings settings;
        [HideInInspector] public PlayerMoveState state = PlayerMoveState.Idle;

        private void Start()
        {

        }
    }

    public enum PlayerMoveState
    {
        Idle,
        Swimming,
        Boosting
    }
}