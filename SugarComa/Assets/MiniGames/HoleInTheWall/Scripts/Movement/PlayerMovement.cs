using Assets.MiniGames.HoleInTheWall.Scripts.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerAnimations _animations;
        [SerializeField] private Transform _orientation;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private CapsuleCollider _capsuleCollider;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _airMultiplier;
        [SerializeField] private float _groundDrag;
        [SerializeField] private float _playerHeight;
        [SerializeField] private LayerMask _whatIsGround;
        private bool _isGrounded;
        private bool _isCrouched;

        private Vector2 _movement;
        private Vector3 _moveDir;

        public Vector2 Movement { get => _movement; }

        public void OnMove(InputAction.CallbackContext obj)
        {
            _movement = obj.ReadValue<Vector2>();
            if (obj.performed) _animations.StartRunning();
            if (obj.canceled) _animations.StopRunning();
        }

        public void OnJump(InputAction.CallbackContext obj)
        {
            if (_isGrounded)
            {
                Jump();
            }
        }

        public void OnCrouch(InputAction.CallbackContext obj)
        {
            if (_isGrounded && !_isCrouched)
            {
                _capsuleCollider.height = 1f;
                _capsuleCollider.center = new Vector3(0, -0.5f, 0);
                _isCrouched = true;
            }
            else if (_isCrouched && obj.canceled)
            {
                _capsuleCollider.height = 2f;
                _capsuleCollider.center = new Vector3(0, 0f, 0);
                _isCrouched = false;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _rigidBody.freezeRotation = true;
        }


        // Update is called once per frame
        void Update()
        {
            CheckGrounded();
            CheckSpeed();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            _moveDir = _orientation.forward * _movement.y + _orientation.right * _movement.x;

            if (_isGrounded)
            {
                _rigidBody.AddForce(_moveDir.normalized * _moveSpeed * 10f, ForceMode.Force);
            }
            else
            {
                _rigidBody.AddForce(_moveDir.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
            }
            
        }

        private void CheckGrounded()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _whatIsGround);

            if (_isGrounded)
            {
                _rigidBody.drag = _groundDrag;
            }
            else
            {
                _rigidBody.drag = 0;
            }
        }

        private void CheckSpeed()
        {
            Vector3 flatVelocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

            if(flatVelocity.magnitude > _moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * _moveSpeed;

                _rigidBody.velocity = new Vector3(limitedVelocity.x, _rigidBody.velocity.y, limitedVelocity.z);
            }
        }
        
        private void Jump()
        {
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

            _rigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        }

        private void Crouch()
        {
            
        }
    }
}