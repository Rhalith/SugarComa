using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerSpecs : MonoBehaviour
    {

        private float _health;
        private bool _isDead;
        private float _moveSpeed = 5f;
        private float _rotationSpeed = 10f;
        private float _localMoveSpeed;
        private float _localRotationSpeed;

        public float Health { get => _health; private set => _health = value; }
        public bool IsDead { get => _isDead; private set => _isDead = value; }
        public float MoveSpeed { get => _moveSpeed; private set => _moveSpeed = value; }
        public float RotationSpeed { get => _rotationSpeed; private set => _rotationSpeed = value; }

        private void Start()
        {
            _localMoveSpeed = MoveSpeed;
            _localRotationSpeed = RotationSpeed;
        }

        public IEnumerator PoisonEffect(int duration, float damage)
        {
            while(duration > 0)
            {
                Health -= damage;
                duration--;
                yield return new WaitForSeconds(1f);
            }
        }

        public void SlowDownPlayer(float ratio)
        {
            ResetPlayerSpeed();
            MoveSpeed /= ratio;
        }

        public void StopPlayerMovement()
        {
            MoveSpeed = 0;
            RotationSpeed = 0;
        }
        
        public void ResetPlayerSpeed()
        {
            MoveSpeed = _localMoveSpeed;
            RotationSpeed = _localRotationSpeed;
        }

        public void DamagePlayer(float damage)
        {
            Health -= damage;
        }
        public void KillPlayer()
        {
            Health = 0;
            IsDead = true;
        }
    }
}