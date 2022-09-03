using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.ThirdPerson
{
    public class PlayerInputReceiver : MonoBehaviour
    {
		[Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool combat;
        public bool grapple;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;

        [Header("References")]
        [SerializeField] private ThirdPersonCam _tpCam;
        [SerializeField] private Grappling _grappling;
        [SerializeField] private PlayerMovement _playerMovement;

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }

        //public void OnJump(InputValue value)
        //{
        //    JumpInput(value.isPressed);
        //}

        //public void OnSprint(InputValue value)
        //{
        //    SprintInput(value.isPressed);
        //}

        public void OnCombat(InputValue value)
        {
            CombatInput(value.isPressed);
            if (value.isPressed)
            {
                _playerMovement.Sprint = false;
                _tpCam.ChangeCamera(ThirdPersonCam.CameraStyle.Combat);
            }
            else
            {
                _playerMovement.Sprint = true;
                _tpCam.ChangeCamera(ThirdPersonCam.CameraStyle.Basic);
            }
        }

        public void OnGrapple(InputValue value)
        {
            GrappleInput(value.isPressed);
            _grappling.StartGrapple();
        }
        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        private void CombatInput(bool newCombatState)
        {
            combat = newCombatState;
        }

        private void GrappleInput(bool newGrappleState)
        {
            grapple = newGrappleState;
        }
        //public void JumpInput(bool newJumpState)
        //{
        //    jump = newJumpState;
        //}

        //public void SprintInput(bool newSprintState)
        //{
        //    sprint = newSprintState;
        //}

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
