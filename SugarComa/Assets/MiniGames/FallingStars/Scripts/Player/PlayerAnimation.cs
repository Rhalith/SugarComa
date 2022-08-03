using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region SerializeField

        [Header("Animation Flags")]
        [SerializeField] private bool _jump;
        [SerializeField] private bool _run;
        [SerializeField] private bool _dead;
        [SerializeField] private bool _hit;

        [Header("Other Scripts")]
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMovement _movement;
        #endregion

        #region Properties
        public bool IsJumping => _jump;
        public bool IsRunning => _run;
        public bool IsDead => _dead;
        public bool IsIdle => !_run && !_jump && !_dead;
        public bool IsHit => _hit;
        #endregion
        private void RunSet(int running)
        {
            _animator.SetBool("running", running != 0);
            _run = running != 0;
        }

        private void JumpSet(int jump)
        {
            _animator.SetBool("jumping", jump != 0);
            _jump = jump != 0;
        }

        private void HitSet(int hit)
        {
            _animator.SetBool("gettinghit", hit != 0);
            _hit = hit != 0;
        }

        public void StartRunning()
        {
            RunSet(1);
        }

        public void StopRunning()
        {
            RunSet(0);
        }

        public void Jump()
        {
            JumpSet(1);
        }

        public void StartGettingHit()
        {
            HitSet(1);
        }

        public void StopGettingHit()
        {
            HitSet(0);
        }
    }
}