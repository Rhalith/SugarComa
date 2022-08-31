using Assets.MainBoard.Scripts.Player.Utils;
using UnityEngine;

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

        public void UpdatePosition(in Vector3 position)
        {
            _t = 0;
            _startPosition = transform.position;
            _nextPosition = position;
            _nextPosition.y = _startPosition.y;
        }
    }
}