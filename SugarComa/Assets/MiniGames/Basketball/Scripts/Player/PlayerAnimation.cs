using Assets.MiniGames.Basketball.Scripts.Ball;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region SerializeField
        [Header("Animation Flags")]
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private bool _aim;
        [SerializeField] private bool _throw;
        [SerializeField] private bool _waiting;
        private BallMovement _ball;

        [Header("Other Scripts")]
        [SerializeField] private Animator _animator;
        #endregion

        #region Properties
        public bool IsWaiting => _waiting;
        public bool IsAiming => _aim;
        public bool IsThrowed => _throw;

        public BallMovement Ball { set => _ball = value; }
        #endregion

        public void Aim()
        {
            AimSet(1);
            ThrowSet(0);
        }

        public void Throw()
        {
            ThrowSet(1);
            AimSet(0);
        }

        public void Waiting()
        {
            WaitingSet(1);
            ThrowSet(0);
        }

        public void ShootBall()
        {
            _ball.ThrowBall();
            StartCoroutine(ResetPlayerAim());
        }
        private void AimSet(int aiming)
        {
            _animator.SetBool("aiming", aiming != 0);
            _aim = aiming != 0;
        }
        private void WaitingSet(int waiting)
        {
            _animator.SetBool("waiting", waiting != 0);
            _waiting = waiting != 0;
        }
        private void ThrowSet(int throws)
        {
            _animator.SetBool("shooting", throws != 0);
            _throw = throws != 0;
        }
        private IEnumerator ResetPlayerAim()
        {
            yield return new WaitForSeconds(1f);
            _playerManager.BallManager.ChangePlayerState(PlayerState.Aiming);
        }
    }
}
