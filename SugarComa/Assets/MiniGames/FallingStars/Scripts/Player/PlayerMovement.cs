using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Properties
        [SerializeField] float _jumpHeight = 1f;
        [SerializeField] float _gravityValue = -9.81f;
        Vector3 _movement;
        Vector3 _movementDir;
        Vector3 _playerVelocity;
        bool _isGrounded;
        bool _isJumping;
        #endregion

        #region OtherComponents
        [SerializeField] CharacterController _characterController;
        [SerializeField] PlayerAnimation _animation;
        [SerializeField] PlayerSpecs _playerSpecs;
        #endregion
        void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.z = Input.GetAxisRaw("Vertical");
            _movementDir = _movement.normalized;
            _isGrounded = _characterController.isGrounded;
        }
        private void FixedUpdate()
        {
            if ((_movement.x != 0 || _movement.z != 0) && _playerSpecs._moveSpeed != 0)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(_movementDir, Vector3.up);
                _characterController.Move(_movementDir * _playerSpecs._moveSpeed * Time.deltaTime);
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
                _characterController.Move(Vector3.forward / 3);
                yield return new WaitForSeconds(0.005f);
                time++;
                _animation.StartGettingHit();
            }
            _animation.StopGettingHit();
        }
    }
}