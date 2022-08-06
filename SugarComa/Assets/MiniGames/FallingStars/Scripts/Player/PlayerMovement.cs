using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Properties
        [SerializeField] float _jumpHeight = 1f;
        [SerializeField] float _gravityValue = -9.81f;
        Vector3 _movement;
        Vector3 _movementDir;
        bool _punch;
        #endregion

        #region OtherComponents
        [SerializeField] PlayerAnimation _animation;
        [SerializeField] PlayerSpecs _playerSpecs;
        PlayerActions _playerInput;
        #endregion

        private void Awake()
        {
            _playerInput = new PlayerActions();

            _playerInput.PlayerInputs.Movement.performed += Movement_performed;
            _playerInput.PlayerInputs.Movement.canceled += Movement_performed;

            _playerInput.PlayerInputs.Punch.started += Punch_performed;
            _playerInput.PlayerInputs.Punch.canceled += Punch_performed;
        }

        private void Punch_performed(InputAction.CallbackContext obj)
        {
            _punch = obj.ReadValueAsButton();
            if (!_punch) _animation.EndToHit();
            else _animation.StartToHit();
        }

        private void Movement_performed(InputAction.CallbackContext obj)
        {
            var input = obj.ReadValue<Vector2>();
            _movement.x = input.x;
            _movement.z = input.y;
            _movementDir = _movement.normalized;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }
        private void FixedUpdate()
        {
            if ((_movement.x != 0 || _movement.z != 0) && _playerSpecs._moveSpeed != 0)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(_movementDir, Vector3.up);
                transform.Translate(_playerSpecs._moveSpeed * Time.deltaTime * _movementDir, Space.World);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs._rotationSpeed * Time.deltaTime);
                _animation.StartRunning();
            }
            else
            {
                _animation.StopRunning();
            }

        }
        /// <summary>
        /// Put it to update otherwise it wont work properly.
        /// </summary>
        //private void Jump()
        //{
        //    if (_isGrounded && _playerVelocity.y < 0)
        //    {
        //        _playerVelocity.y = 0f;
        //        if (_isJumping)
        //        {
        //            _animation.Jump();
        //            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        //        }
        //    }
        //    _playerVelocity.y += _gravityValue * Time.deltaTime;
        //    _characterController.Move(_playerVelocity * Time.deltaTime);
        //}

        public void Aduket()
        {
            StartCoroutine(aduket());
        }
        IEnumerator aduket()
        {
            float time = 0;
            while(time < 60)
            {
                transform.Translate(-Vector3.forward / 3);
                yield return new WaitForSeconds(0.005f);
                time++;
                _animation.StartGettingHit();
            }
            _animation.StopGettingHit();
        }
    }
}