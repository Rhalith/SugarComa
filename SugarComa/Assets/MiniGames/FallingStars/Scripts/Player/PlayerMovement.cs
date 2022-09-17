using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Serialized Field
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private PlayerSpecifications _playerSpecs;
        #endregion

        #region Private Fields
        private Vector3 _movement;
        private Vector3 _movementDir;
        private Vector3 _rotationDir;
        private Vector3 _mouseDir;
        private bool _punch;
        private bool _isMouseActive;
        private bool _isGamepadActive;
        private RaycastHit _hit;
        private Ray _ray;
        private PlayerActions _playerInput; 
        private Camera _mainCam;
        #endregion

        #region Properties
        public PlayerSpecifications PlayerSpec { get => _playerSpecs; }
        public PlayerActions PlayerInput { get => _playerInput; private set => _playerInput = value; }
        public Camera MainCam { get => _mainCam; set => _mainCam = value; }
        #endregion

        private void Awake()
        {
            PlayerInput = new PlayerActions();

            PlayerInput.PlayerInputs.Movement.performed += Movement_performed;
            PlayerInput.PlayerInputs.Movement.canceled += Movement_performed;

            //_playerInput.PlayerInputs.Punch.started += Punch_started;
            //_playerInput.PlayerInputs.Punch.canceled += Punch_started;

            PlayerInput.PlayerInputs.RotationWithGamepad.performed += RotationWithGamepad_performed;
            PlayerInput.PlayerInputs.RotationWithGamepad.canceled += RotationWithGamepad_performed;

            PlayerInput.PlayerInputs.RotationWithMouse.started += RotationWithMouse_performed;
            PlayerInput.PlayerInputs.RotationWithMouse.performed += RotationWithMouse_performed;
            PlayerInput.PlayerInputs.RotationWithMouse.canceled += RotationWithMouse_performed;

            PlayerInput.PlayerInputs.MouseForRotation.performed += MouseForRotation_performed;
            PlayerInput.PlayerInputs.MouseForRotation.canceled += MouseForRotation_performed;
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
            _rotationDir.x = -input.x;
            _rotationDir.z = -input.y;
            _rotationDir = _rotationDir.normalized;
            _isGamepadActive = obj.performed;
        }

        //private void Punch_started(InputAction.CallbackContext obj)
        //{
        //    _punch = obj.ReadValueAsButton();
        //    if (!_punch) _animation.EndToHit();
        //    else _animation.StartToHit();
        //}

        private void Movement_performed(InputAction.CallbackContext obj)
        {
            var input = obj.ReadValue<Vector2>();
            _movement.x = -input.x;
            _movement.z = -input.y;
            _movementDir = _movement.normalized;
        }

        private void OnEnable()
        {
            PlayerInput.Enable();
        }

        private void OnDisable()
        {
            PlayerInput.Disable();
        }
        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            if(_playerSpecs.MoveSpeed > 0)
            {
                RotatePlayer();
                TranslatePlayer();
            }
            else
            {
                _animation.StopRunning();
            }
        }
        private void RotatePlayer()
        {
            if (_isMouseActive)
            {
                RotateWithMouse(_mouseDir);
            }
            else if (_isGamepadActive)
            {
                RotateWithGamepad(_rotationDir);
            }
            else if (_movementDir.x != 0 || _movementDir.z != 0)
            {
                RotateWithMovement(_movementDir);
            }
        }

        private void TranslatePlayer()
        {
            if (_movement.x != 0 || _movement.z != 0)
            {
                transform.Translate(_playerSpecs.MoveSpeed * Time.deltaTime * _movementDir, Space.World);
                _animation.StartRunning();
            }
            else
            {
                _animation.StopRunning();
            }
        }

        private void RotateWithMouse(Vector3 direction)
        {
            _ray = _mainCam.ScreenPointToRay(direction);
            if (Physics.Raycast(_ray, out _hit))
            {
                if (!_hit.collider.CompareTag("Player"))
                {
                    Quaternion desiredRotation = Quaternion.LookRotation(new Vector3(_hit.point.x - transform.position.x, 0, _hit.point.z - transform.position.z), Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs.RotationSpeed * Time.deltaTime);
                }
            }
        }

        private void RotateWithGamepad(Vector3 direction)
        {
            if (direction.x != 0 || direction.z != 0)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs.RotationSpeed * Time.deltaTime);
            }
        }

        private void RotateWithMovement(Vector3 direction)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs.RotationSpeed * Time.deltaTime);
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
        //public void Aduket()
        //{
        //    StartCoroutine(aduket());
        //}
        //IEnumerator aduket()
        //{
        //    float time = 0;
        //    while(time < 60)
        //    {
        //        transform.Translate(-Vector3.forward / 3);
        //        yield return new WaitForSeconds(0.005f);
        //        time++;
        //        _animation.StartGettingHit();
        //    }
        //    _animation.StopGettingHit();
        //}
    }
}