using Assets.MainBoard.Scripts.Networking;
using Assets.MiniGames.FallingStars.Scripts.Networking.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.FallingStars.Scripts.Player.Remote
{
    public class RemotePlayerMovement : MonoBehaviour
    {
        #region Serialized Field
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private PlayerSpecifications _playerSpecs;
        #endregion

        #region Private Fields
        private Vector3 _movementDir;
        private Vector3 _rotationDir;
        #endregion

        #region Properties
        public PlayerSpecifications PlayerSpec { get => _playerSpecs; }
        public Vector3 MovementDir { get => _movementDir; set => _movementDir = value; }
        public Vector3 RotationDir { get => _rotationDir; set => _rotationDir = value; }
        #endregion
        
        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            if(_playerSpecs.MoveSpeed > 0)
            {
                TranslatePlayer();
                RotatePlayer();
            }
            else
            {
                _animation.StopRunning();
            }
        }
        private void RotatePlayer()
        {
            Rotate(_rotationDir);
        }

        private void TranslatePlayer()
        {
            if (_movementDir.x != 0 || _movementDir.z != 0)
            {
                transform.Translate(_playerSpecs.MoveSpeed * Time.deltaTime * _movementDir, Space.World);
                _animation.StartRunning();
            }
            else
            {
                _animation.StopRunning();
            }
        }

        private void Rotate(Vector3 direction)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _playerSpecs.RotationSpeed * Time.deltaTime);
        }
    }
}