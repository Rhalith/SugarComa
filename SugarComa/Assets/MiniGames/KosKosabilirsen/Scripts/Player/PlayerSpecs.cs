using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.Player
{
    public class PlayerSpecs : MonoBehaviour
    {
        [SerializeField] private int _playerHealth;
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] private float _playerMoveSpeed;

        [Tooltip("Sprint speed of the character in m/s")]
        [SerializeField] private float _playerSlowSpeed;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float _playerRotationSpeed;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float _speedChangeRate = 10.0f;
        private bool _isSlow;

        public int PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
        public float PlayerMoveSpeed { get => _playerMoveSpeed; set => _playerMoveSpeed = value; }
        public float PlayerRotationSpeed { get => _playerRotationSpeed; set => _playerRotationSpeed = value; }
        public float PlayerSlowSpeed { get => _playerSlowSpeed; set => _playerSlowSpeed = value; }
        public bool IsSlow { get => _isSlow; set => _isSlow = value; }
        public float SpeedChangeRate { get => _speedChangeRate; set => _speedChangeRate = value; }
    }
}