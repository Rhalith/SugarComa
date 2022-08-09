using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerSpecifications : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _health = 25f;
        private bool _isDead;
        private float _localMoveSpeed;
        private float _localRotationSpeed;

        public float Health { get => _health; set => _health = value; }
        public bool IsDead { get => _isDead; set => _isDead = value; }
        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
        public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
        public float LocalMoveSpeed { get => _localMoveSpeed; set => _localMoveSpeed = value; }
        public float LocalRotationSpeed { get => _localRotationSpeed; set => _localRotationSpeed = value; }

        private void Start()
        {
            LocalMoveSpeed = MoveSpeed;
            LocalRotationSpeed = RotationSpeed;
        }
    }
}