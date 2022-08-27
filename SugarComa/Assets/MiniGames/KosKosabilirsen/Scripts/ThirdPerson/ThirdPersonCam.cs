using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.ThirdPerson
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _orientation;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _playerObject;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private PlayerInputReceiver _playerInputReceiver;

        public float rotationSpeed;

        public Transform combatLookAt;

        public CameraStyle currentStyle;

        public enum CameraStyle
        {
            Basic,
            Combat
        }

        void Update()
        {
            RotatePlayer(currentStyle);
        }

        private void RotatePlayer(CameraStyle style)
        {
            if (style.Equals(CameraStyle.Basic))
            {
                Vector3 viewDirection = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
                _orientation.forward = viewDirection.normalized;

                Vector3 inputDirection = _orientation.forward * _playerInputReceiver.move.y + _orientation.right * _playerInputReceiver.move.x;

                if (inputDirection != Vector3.zero)
                {
                    _playerObject.forward = Vector3.Slerp(_playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            else if (style.Equals(CameraStyle.Combat))
            {
                Vector3 combatDirection = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                _orientation.forward = combatDirection.normalized;

                _playerObject.forward = combatDirection.normalized;
            }

        }
    }
}