using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.ThirdPerson
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _groundDrag;
        [SerializeField] private float _airMultiplier;

        [SerializeField] private Transform _orientation;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private PlayerInputReceiver _playerInputReceiver;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        public bool grounded;

        private void FixedUpdate()
        {
            MovePlayer();
        }
        private void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            SpeedControl();

            if (grounded)
            {
                _rb.drag = _groundDrag;
            }
            else
            {
                _rb.drag = 0;
            }
        }

        Vector3 moveDirection;
        private void MovePlayer()
        {
            moveDirection = _orientation.forward * _playerInputReceiver.move.y + _orientation.right * _playerInputReceiver.move.x;

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
            Vector3 currentVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if(currentVelocity.magnitude > _moveSpeed)
            {
                Vector3 limitedVelocity = currentVelocity.normalized * _moveSpeed;

                _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
            }
        }


    }

}