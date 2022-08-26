using UnityEngine;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.Utils;
using System.Linq;

namespace Assets.MainBoard.Scripts.Player.Remote
{
    public class RemotePlayerMovement : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _rotationSpeed = 720;
        [SerializeField] private RemotePlayerAnimation _playerAnimation;
        [SerializeField] private RemoteScriptKeeper _scKeeper;
        #endregion

        #region Private Fields
        private Vector3 _startPosition;
        private Vector3 _nextPosition;
        private float _t;
        #endregion

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
            // Check for sent data if it's belong to this remote player object
            if (NetworkManager.Instance.playerList.ElementAt(_scKeeper.playerIndex).Key != steamid)
                return;

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