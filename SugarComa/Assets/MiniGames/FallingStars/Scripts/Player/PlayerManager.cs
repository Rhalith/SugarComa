using Assets.MiniGames.FallingStars.Scripts.Meteors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerManager: MonoBehaviour
    {
        private ClassicMeteor _classicMeteor;
        private ExplosionMeteor _explosionMeteor;
        private StickyMeteor _stickyMeteor;
        private PoisonMeteor _poisonMeteor;
        private Punch _punch;

        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerHit _playerHit;

        private void Awake()
        {
            _classicMeteor = new(this);
            _explosionMeteor = new(this);
            _stickyMeteor = new(this);
            _poisonMeteor = new(this);
            _punch = new(this);
        }

        public void DamagePlayer(float damage)
        {
            _playerMovement.PlayerSpec.Health -= damage;
            if (_playerMovement.PlayerSpec.Health <= 0) KillPlayer();
        }
        public void KillPlayer()
        {
            _playerMovement.PlayerSpec.Health = 0;
            _playerMovement.PlayerSpec.IsDead = true;
        }
        public void GetHit(Transform attackPlayer, Transform hitPlayer)
        {
            print("gettinghit");
            StartCoroutine(_punch.GetHit(attackPlayer.forward, hitPlayer, _playerAnimation, _playerMovement.PlayerSpec));
        }
        public void StartNumerator(MeteorType meteorType, float damage = 0, int duration = 0, float ratio = 0)
        {
            switch (meteorType)
            {
                case MeteorType.classic:
                    _classicMeteor._coroutine = StartCoroutine(_classicMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.explosion:
                    _explosionMeteor._coroutine = StartCoroutine(_explosionMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.poison:
                    _poisonMeteor.StopIteration();
                    _poisonMeteor._coroutine = StartCoroutine(_poisonMeteor.DamageToPlayer(damage));
                    break;
                case MeteorType.sticky:
                    _stickyMeteor._coroutine = StartCoroutine(_stickyMeteor.StopPlayer(duration, ratio));
                    break;
            }
        }
        public void StopNumerator(MeteorType meteorType, float damage = 0, int duration = 0)
        {
            switch (meteorType)
            {
                case MeteorType.classic:
                    _classicMeteor.StopIteration();
                    break;
                case MeteorType.explosion:
                    _explosionMeteor.StopIteration();
                    break;
                case MeteorType.poison:
                    _poisonMeteor.StopIteration();
                    _poisonMeteor._coroutine = StartCoroutine(_poisonMeteor.StartPoisonEffect(duration, damage));
                    break;
                case MeteorType.sticky:
                    _stickyMeteor.StopIteration();
                    _stickyMeteor.ResetPlayerSpeed();
                    break;

            }
        }
        #region ClassicMeteor

        private class ClassicMeteor
        {
            public Coroutine _coroutine;
            private readonly PlayerManager _playerManager;

            public ClassicMeteor(PlayerManager playerManager)
            {
                _playerManager = playerManager;
            }

            public IEnumerator DamageToPlayer(float damage = 0)
            {
                while (_playerManager._playerMovement.PlayerSpec.Health > 0)
                {
                    _playerManager.DamagePlayer(damage);
                    yield return new WaitForSeconds(1f);
                }
            }
            public void StopIteration()
            {
                _playerManager.StopCoroutine(_coroutine);
            }
        }
        #endregion
        #region ExplosionMeteor
        private class ExplosionMeteor
        {
            public Coroutine _coroutine;
            private readonly PlayerManager _playerManager;

            public ExplosionMeteor(PlayerManager playerManager)
            {
                _playerManager = playerManager;
            }

            public IEnumerator DamageToPlayer(float damage = 0)
            {
                while (true)
                {
                    _playerManager.DamagePlayer(damage);
                    yield return new WaitForSeconds(1f);
                }
            }
            public void StopIteration()
            {
                _playerManager.StopCoroutine(_coroutine);
            }
        }
        #endregion
        #region StickyMeteor
        private class StickyMeteor
        {
            public Coroutine _coroutine;
            private readonly PlayerManager _playerManager;

            public StickyMeteor(PlayerManager playerManager)
            {
                _playerManager = playerManager;
            }
            public IEnumerator StopPlayer(int duration = 0, float ratio = 0)
            {
                StopPlayerMovement();
                yield return new WaitForSeconds(duration);
                SlowDownPlayer(ratio);
            }
            public void SlowDownPlayer(float ratio)
            {
                ResetPlayerSpeed();
                _playerManager._playerMovement.PlayerSpec.MoveSpeed /= ratio;
            }
            public void StopPlayerMovement()
            {
                _playerManager._playerMovement.PlayerSpec.MoveSpeed = 0;
                _playerManager._playerMovement.PlayerSpec.RotationSpeed = 0;
            }
            public void ResetPlayerSpeed()
            {
                _playerManager._playerMovement.PlayerSpec.MoveSpeed = _playerManager._playerMovement.PlayerSpec.LocalMoveSpeed;
                _playerManager._playerMovement.PlayerSpec.RotationSpeed = _playerManager._playerMovement.PlayerSpec.LocalRotationSpeed;
            }
            public void StopIteration()
            {
                _playerManager.StopCoroutine(_coroutine);
            }
        }

        #endregion
        #region PoisonMeteor
        private class PoisonMeteor
        {
            public Coroutine _coroutine;
            private readonly PlayerManager _playerManager;

            public PoisonMeteor(PlayerManager playerManager)
            {
                _playerManager = playerManager;
            }
            public IEnumerator DamageToPlayer(float damage = 0)
            {
                while (_playerManager._playerMovement.PlayerSpec.Health > 0)
                {
                    _playerManager.DamagePlayer(damage);
                    yield return new WaitForSeconds(1f);
                }
            }
            public IEnumerator StartPoisonEffect(int duration, float damage)
            {
                while (duration > 0)
                {
                    _playerManager.DamagePlayer(damage);
                    duration--;
                    yield return new WaitForSeconds(1f);
                }
            }
            public void StopIteration()
            {
                _playerManager.StopCoroutine(_coroutine);
            }
        }

        #endregion
        #region Punch
        private class Punch
        {
            public Coroutine _corouite;
            private readonly PlayerManager _playerManager;
            public Punch(PlayerManager playerManager)
            {
                _playerManager = playerManager;
            }
            public IEnumerator GetHit(Vector3 direction, Transform transform, PlayerAnimation playerAnimation, PlayerSpecifications playerSpec, float time = 60f,  float waitTime = 0.005f)
            {
                StopPlayerMovement(true, playerSpec);
                Quaternion desiredRotation = Quaternion.LookRotation(-direction, Vector3.up);
                transform.rotation = desiredRotation;
                while (time > 0)
                {
                    transform.Translate(-transform.forward / playerSpec.PunchRatio, Space.World);
                    yield return new WaitForSeconds(waitTime);
                    time--;
                    playerAnimation.StartGettingHit();
                }
                playerAnimation.StopGettingHit();
                StopPlayerMovement(false, playerSpec);
            }

            private void StopPlayerMovement(bool isStop, PlayerSpecifications playerSpec)
            {
                if (isStop)
                {
                    playerSpec.MoveSpeed = 0;
                }
                else
                {
                    playerSpec.MoveSpeed = playerSpec.LocalMoveSpeed;
                }
            }
        }
        #endregion
    }
}