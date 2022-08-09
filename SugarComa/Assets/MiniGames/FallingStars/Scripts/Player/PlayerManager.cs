using Assets.MiniGames.FallingStars.Scripts.Meteors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerManager: MonoBehaviour
    {
        [SerializeField] private PlayerSpecifications _playerSpec;
        private ClassicMeteor _classicMeteor = new();
        private ExplosionMeteor _explosionMeteor = new();
        private StickyMeteor _stickyMeteor = new();
        private PoisonMeteor _poisonMeteor = new();
        private void Start()
        {

        }
        public void DamagePlayer(float damage)
        {
            _playerSpec.Health -= damage;
            if (_playerSpec.Health <= 0) KillPlayer();
        }
        public void KillPlayer()
        {
            _playerSpec.Health = 0;
            _playerSpec.IsDead = true;
        }
        public void StartNumerator(MeteorType meteorType, float damage = 0, int duration = 0, float ratio = 0)
        {
            switch (meteorType)
            {
                case MeteorType.classic:
                    StartCoroutine(_classicMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.explosion:
                    StartCoroutine(_explosionMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.poison:
                    StopCoroutine(_poisonMeteor.StartPoisonEffect(duration, damage));
                    StartCoroutine(_poisonMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.sticky:
                    StartCoroutine(_stickyMeteor.StopPlayer(duration, ratio));
                    break;
            }
        }
        public void StopNumerator(MeteorType meteorType, float damage = 0, int duration = 0, float ratio = 0)
        {
            switch (meteorType)
            {
                case MeteorType.classic:
                    StopCoroutine(_classicMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.explosion:
                    StopCoroutine(_explosionMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.poison:
                    StopCoroutine(_poisonMeteor.DamageToPlayer(damage));
                    StartCoroutine(_poisonMeteor.StartPoisonEffect(duration, damage));
                    break;
                case MeteorType.sticky:
                    StopCoroutine(_stickyMeteor.StopPlayer(duration, ratio));
                    _stickyMeteor.ResetPlayerSpeed();
                    break;

            }
        }
        #region ClassicMeteor

        private class ClassicMeteor : PlayerManager
        {
            public IEnumerator DamageToPlayer(float damage = 0)
            {
                while (_playerSpec.Health > 0)
                {
                    DamagePlayer(damage);
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        #endregion
        #region ExplosionMeteor
        private class ExplosionMeteor : PlayerManager
        {
            public IEnumerator DamageToPlayer(float damage = 0)
            {
                while (true)
                {
                    DamagePlayer(damage);
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        #endregion
        #region StickyMeteor
        private class StickyMeteor : PlayerManager
        {
            public IEnumerator StopPlayer(int duration = 0, float ratio = 0)
            {
                StopPlayerMovement();
                yield return new WaitForSeconds(duration);
                SlowDownPlayer(ratio);
            }
            public void SlowDownPlayer(float ratio)
            {
                ResetPlayerSpeed();
                _playerSpec.MoveSpeed /= ratio;
            }
            public void StopPlayerMovement()
            {
                _playerSpec.MoveSpeed = 0;
                _playerSpec.RotationSpeed = 0;
            }
            public void ResetPlayerSpeed()
            {
                _playerSpec.MoveSpeed = _playerSpec.LocalMoveSpeed;
                _playerSpec.RotationSpeed = _playerSpec.LocalRotationSpeed;
            }
        }

        #endregion
        #region PoisonMeteor
        private class PoisonMeteor : PlayerManager
        {
            public IEnumerator DamageToPlayer(float damage = 0)
            {
                while (_playerSpec.Health > 0)
                {
                    DamagePlayer(damage);
                    yield return new WaitForSeconds(1f);
                }
            }
            public IEnumerator StartPoisonEffect(int duration, float damage)
            {
                while (duration > 0)
                {
                    DamagePlayer(damage);
                    duration--;
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        #endregion
    }
}