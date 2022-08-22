using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.MiniGames.KosKosabilirsen.Scripts.Input;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerInputs _playerInput;

        private Vector3 _movementDirection;
        private bool _isJumping;

        private void Awake()
        {
            _playerInput = new();


            _playerInput.Movement.Move.performed += Move_performed;
            _playerInput.Movement.Jump.started += Jump_started;
        }

        private void Jump_started(InputAction.CallbackContext obj)
        {
            _isJumping = obj.performed;
        }

        private void Move_performed(InputAction.CallbackContext obj)
        {
            Vector2 direction = obj.ReadValue<Vector2>();
            _movementDirection.x = direction.x;
            _movementDirection.z = direction.y;
        }
    }
}