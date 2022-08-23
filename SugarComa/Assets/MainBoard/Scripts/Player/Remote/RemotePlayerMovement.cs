using UnityEngine;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;

namespace Assets.MainBoard.Scripts.Player.Remote
{
    public class RemotePlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _rotationSpeed = 720;
        [SerializeField] private RemotePlayerAnimation _playerAnimation;

        private Vector3 _startPosition;
        private Vector3 _nextPosition;
        private float _t;

        private void Start()
        {
            SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;

        }
        private void OnDestroy()
        {
            SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
        }

        void Update()
        {
            if (_nextPosition == transform.position)
            {
                return;
            }

            _t += Time.deltaTime * _speed;
            // Smooth tracking
            // if object position not equal the current platform position move to position.
            transform.position = Vector3.Lerp(_startPosition, _nextPosition, _t);

            // rotation
            Vector3 movementDirection = (_nextPosition - transform.position).normalized;
            if (movementDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
        {
            if (!NetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
                return;

            if (networkData.type == MessageType.InputDown)
            {
                _t = 0;
                _startPosition = transform.position;
                _nextPosition = networkData.position;
                _nextPosition.y = _startPosition.y;
            }
        }
    }
}