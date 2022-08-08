using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerSpecs : MonoBehaviour
    {

        public float _health;
        public bool _isDead;
        public float _moveSpeed = 5f;
        public float _rotationSpeed = 10f;

        private float _localMoveSpeed;
        private float _localRotationSpeed;

        private void Start()
        {
            _localMoveSpeed = _moveSpeed;
            _localRotationSpeed = _rotationSpeed;
        }

        public IEnumerator PoisonEffect(int duration, float damage)
        {
            while(duration > 0)
            {
                _health -= damage;
                duration--;
                yield return new WaitForSeconds(1f);
            }
        }

        public void SlowDownPlayer(float ratio)
        {
            ResetPlayerSpeed();
            _moveSpeed /= ratio;
        }

        public void StopPlayerMovement()
        {
            _moveSpeed = 0;
            _rotationSpeed = 0;
        }
        
        public void ResetPlayerSpeed()
        {
            _moveSpeed = _localMoveSpeed;
            _rotationSpeed = _localRotationSpeed;
        }

        public void DamagePlayer(float damage)
        {
            _health -= damage;
        }
        public void KillPlayer()
        {
            _health = 0;
            _isDead = true;
        }
    }
}