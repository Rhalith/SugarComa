using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Properties
        #region Serialized Field
        //[SerializeField] private float _jumpHeight = 1f;
        //[SerializeField] private float _gravityValue = -9.81f;
        [SerializeField] private Camera _cam;
        #endregion
        private Vector3 _movement;
        private Vector3 _movementDir;
        private Vector3 _rotationDir;
        private Vector3 _mouseDir;
        private bool _punch;
        private bool _isMouseActive;
        private RaycastHit _hit;
        private Ray _ray;
        #endregion

        #region OtherComponents
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private PlayerSpecifications _playerSpecs;
        private PlayerActions _playerInput;
        public PlayerSpecifications PlayerSpec { get => _playerSpecs; }
        #endregion

        private void Awake()
        {
            _playerInput = new PlayerActions();

            _playerInput.PlayerInputs.Movement.performed += Movement_performed;
            _playerInput.PlayerInputs.Movement.canceled += Movement_performed;

            _playerInput.PlayerInputs.Punch.started += Punch_started;
            _playerInput.PlayerInputs.Punch.canceled += Punch_started;

            _playerInput.PlayerInputs.RotationWithGamepad.performed += RotationWithGamepad_performed;
            _playerInput.PlayerInputs.RotationWithGamepad.canceled += RotationWithGamepad_performed;

            _playerInput.PlayerInputs.RotationWithMouse.started += RotationWithMouse_performed;
            _playerInput.PlayerInputs.RotationWithMouse.performed += RotationWithMouse_performed;
            _playerInput.PlayerInputs.RotationWithMouse.canceled += RotationWithMouse_performed;

            _playerInput.PlayerInputs.MouseForRotation.performed += MouseForRotation_performed;
            _playerInput.PlayerInputs.MouseForRotation.canceled += MouseForRotation_performed;
        }

        private void RotationWithMouse_performed(InputAction.CallbackContext obj)
        {
            _mouseDir = obj.ReadValue<Vector2>();
        }

        private void MouseForRotation_performed(InputAction.CallbackContext obj)
        {
            _isMouseActive = obj.performed;
        }

        private void RotationWithGamepad_performed(InputAction.CallbackContext obj)
        {
            var input = obj.ReadValue<Vector2>();
            _rotationDir.x = input.x;
            _rotationDir.z = input.y;
            _rotationDir = _rotationDir.normalized;
        }

        private void Punch_started(InputAction.CallbackContext obj)
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
        //TODO düzenle
        private void FixedUpdate()
        {
            if(_playerSpecs.MoveSpeed != 0)
            {
                if (!_isMouseActive)
                {
                    if(_rotationDir.x != 0 || _rotationDir.z != 0)
                    {
                        Quaternion desiredRotation = Quaternion.LookRotation(_rotationDir, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs.RotationSpeed * Time.deltaTime);
                    }
                    else if (_movementDir.x != 0 || _movementDir.z != 0)
                    {
                        Quaternion desiredRotation = Quaternion.LookRotation(_movementDir, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs.RotationSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    //TODO
                    _ray = _cam.ScreenPointToRay(_mouseDir);
                    if (Physics.Raycast(_ray, out _hit))
                    {
                        Quaternion desiredRotation = Quaternion.LookRotation(new Vector3(_hit.point.x, 0, _hit.point.z), Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs.RotationSpeed * Time.deltaTime);
                    }
                }
                transform.Translate(_playerSpecs.MoveSpeed * Time.deltaTime * _movementDir, Space.World);
                if ((_movement.x != 0 || _movement.z != 0))
                {
                    _animation.StartRunning();
                }
                else
                {
                    _animation.StopRunning();
                }

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

        //TODO
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