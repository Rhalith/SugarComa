using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Animations
{
    public class PlayerAnimations : MonoBehaviour
    {
        #region SerializeField

        [Header("Animation Flags")]
        [SerializeField] private bool _jump;
        [SerializeField] private bool _run;
        [SerializeField] private bool _dead;

        [Header("Other Scripts")]
        [SerializeField] private Animator _animator;
        #endregion

        #region Properties
        public bool IsJumping => _jump;
        public bool IsRunning => _run;
        public bool IsDead => _dead;
        public bool IsIdle => !_run && !_jump && !_dead;
        #endregion
        private void RunSet(int running)
        {
            _animator.SetBool("isRunning", running != 0);
            _run = running != 0;
        }

        private void JumpSet(int jump)
        {
            _animator.SetBool("isJumping", jump != 0);
            _jump = jump != 0;
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
    }
}