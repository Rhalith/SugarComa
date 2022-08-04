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

        public float _localMoveSpeed;
        public float _localRotationSpeed;

        public IEnumerator PoisonEffect(int duration, float damage)
        {
            while(duration < 5)
            {
                _health -= damage;
                duration++;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}