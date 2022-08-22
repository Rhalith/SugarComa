using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.Player
{
    public class PlayerSpecs : MonoBehaviour
    {
        [SerializeField] private int _playerHealth;
        [SerializeField] private float _playerMoveSpeed;
        [SerializeField] private float _playerRotationSpeed;
        private float _localMoveSpeed;

        public int PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
        public float PlayerMoveSpeed { get => _playerMoveSpeed; set => _playerMoveSpeed = value; }
        public float PlayerRotationSpeed { get => _playerRotationSpeed; set => _playerRotationSpeed = value; }

        private void Awake()
        {
            _localMoveSpeed = _playerMoveSpeed;
        }
        /// <summary>
        /// PlayerSpeed returns normal.
        /// </summary>
        public void ResetPlayerSpeed()
        {
            _playerMoveSpeed = _localMoveSpeed;
        }
        /// <summary>
        /// PlayerSpeed multiplies with ratio.
        /// </summary>
        /// <param name="ratio"></param>
        public void ChangePlayerSpeed(float ratio)
        {
            _playerMoveSpeed *= ratio;
        }
    }
}