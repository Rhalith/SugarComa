using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Assets.MainBoard.Scripts.Utils.CamUtils
{
    public class MapCamera : MonoBehaviour
    {
        [SerializeField] public CinemachineVirtualCamera _camera, _mainCamera;
        [SerializeField] CharacterController _controller;

        [SerializeField] Transform _player;

        [SerializeField] float speed = 6f;

        public void SetCameraPriority(CinemachineVirtualCamera camera, int value, bool isClose = false)
        {
            if (!isClose) GoToPlayer();
            camera.Priority = value;
        }

        private void FixedUpdate()
        {
            if (_camera.Priority > _mainCamera.Priority)
            {
                float vertical = Input.GetAxisRaw("Horizontal");
                float horizontal = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3(horizontal, 0f, -vertical).normalized;

                if (direction.magnitude >= 0.1f)
                {
                    _controller.Move(direction.normalized * speed * Time.deltaTime);
                }
            }
        }
        private void GoToPlayer()
        {
            transform.position = _player.transform.position;
        }
    }
}