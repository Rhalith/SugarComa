using Assets.MiniGames.Basketball.Scripts.Ball;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region SerializeField
        [Header("Animation Flags")]
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private bool _aim;
        [SerializeField] private bool _throw;
        private BallMovement _ball;

        [Header("Other Components")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Animator _ballAnimator;
        [SerializeField] private MeshRenderer _ballRenderer;
        #endregion

        #region Properties
        public bool IsAiming => _aim;
        public bool IsThrowed => _throw;

        public BallMovement Ball { get => _ball; set => _ball = value; }
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

        public void ShootBall()
        {
            _ballRenderer.enabled = false;
            _ballAnimator.enabled = false;
            _ball.ThrowBall();
            _ball = null;
            StartCoroutine(ResetPlayerAim());
        }
        private void AimSet(int aiming)
        {
            _playerAnimator.SetBool("aiming", aiming != 0);
            _ballAnimator.enabled = true;
            _ballRenderer.enabled = true;
            _aim = aiming != 0;
        }
        private void ThrowSet(int throws)
        {
            _playerAnimator.SetBool("shooting", throws != 0);
            _ballAnimator.SetBool("shooting", throws != 0);
            _throw = throws != 0;
        }
        private IEnumerator ResetPlayerAim()
        {
            yield return new WaitForSeconds(1f);
            _ball = _playerManager.BallManager.GetBall();
            if (_ball == null)
            {
                StartCoroutine(ResetPlayerAim());
                yield return null;
            }
            _playerManager.PlayerAiming();
        }
    }
}
