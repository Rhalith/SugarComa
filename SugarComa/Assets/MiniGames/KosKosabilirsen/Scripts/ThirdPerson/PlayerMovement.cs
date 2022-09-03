using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.ThirdPerson
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _groundDrag;
        [SerializeField] private float _airMultiplier;
        private float _moveSpeed;
        private Vector3 moveDirection;

        [SerializeField] private Transform _orientation;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private PlayerInputReceiver _playerInputReceiver;
        [SerializeField] private Grappling _grappling;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        public bool grounded;

        [Header("State")]
        public MovementState state;
        private bool _freeze;
        private bool _sprint;
        private bool _isGrappleActive;
        private bool _enableMovement;
        private Vector3 _velocityToSet;

        public bool Freeze { get => _freeze; set => _freeze = value; }
        public bool Sprint { get => _sprint; set => _sprint = value; }

        public enum MovementState
        {
            freeze,
            walking,
            sprinting
        }


        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_enableMovement)
            {
                _enableMovement = false; ResetRestrictions(); _grappling.StopGrapple();
            }
        }

        public void ResetRestrictions()
        {
            _isGrappleActive = false;
        }
        private void StateHandler()
        {
            //Mode - Freeze
            if (_freeze)
            {
                state = MovementState.freeze;
                _moveSpeed = 0;
                _rb.velocity = Vector3.zero;
            }
            else if (_sprint)
            {
                state = MovementState.sprinting;
            }
            else
            {
                state = MovementState.walking;
            }
        }
        private void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            SpeedControl();
            StateHandler();

            if (grounded && !_isGrappleActive)
            {
                _rb.drag = _groundDrag;
            }
            else
            {
                _rb.drag = 0;
            }
        }

        public void JumpToPosition(Vector3 target, float trajectoryHeight)
        {
            _isGrappleActive = true;
            _velocityToSet = CalculateJumpVelocity(transform.position, target, trajectoryHeight);
            Invoke(nameof(SetVelocity), 0.1f);

            //in case of something went wrong
            Invoke(nameof(ResetRestrictions), 3f);
        }

        private void SetVelocity()
        {
            _enableMovement = true;
            _rb.velocity = _velocityToSet;
        }
        private void MovePlayer()
        {
            if (_isGrappleActive)
            {
                return;
            }
            moveDirection = _orientation.forward * _playerInputReceiver.move.y + _orientation.right * _playerInputReceiver.move.x;
            _moveSpeed = _sprint ? _sprintSpeed : _walkSpeed;

            if (grounded)
            {
                _rb.AddForce(moveDirection.normalized * _moveSpeed * 20f, ForceMode.Force);
            }
            else
            {
                _rb.AddForce(moveDirection.normalized * _moveSpeed * 20f* _airMultiplier, ForceMode.Force);
            }
        }

        private void SpeedControl()
        {
            if (_isGrappleActive)
            {
                return;
            }
            Vector3 currentVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if(currentVelocity.magnitude > _moveSpeed)
            {
                Vector3 limitedVelocity = currentVelocity.normalized * _moveSpeed;

                _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
            }
        }

        private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = Physics.gravity.y;
            float displacementY = endPoint.y - startPoint.y;
            Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
            Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
                + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

            return velocityXZ + velocityY;
        }
    }

}