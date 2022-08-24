using UnityEngine;
using Cinemachine;

namespace Assets.MainBoard.Scripts.Utils.CamUtils
{
    public class MapCamera : MonoBehaviour
    {
        public CinemachineVirtualCamera cam, mainCamera;
        public Transform player;

        [SerializeField] CharacterController _controller;
        [SerializeField] float speed = 6f;

        public void SetCameraPriority(CinemachineVirtualCamera camera, int value, bool isClose = false)
        {
            if (!isClose) GoToPlayer();
            camera.Priority = value;
        }

        private void FixedUpdate()
        {
            //if (cam.Priority > mainCamera.Priority)
            //{
            //    float vertical = Input.GetAxisRaw("Horizontal");
            //    float horizontal = Input.GetAxisRaw("Vertical");
            //    Vector3 direction = new Vector3(horizontal, 0f, -vertical).normalized;

            //    if (direction.magnitude >= 0.1f)
            //    {
            //        _controller.Move(direction.normalized * speed * Time.deltaTime);
            //    }
            //}
        }

        private void GoToPlayer()
        {
            transform.position = player.transform.position;
        }
    }
}