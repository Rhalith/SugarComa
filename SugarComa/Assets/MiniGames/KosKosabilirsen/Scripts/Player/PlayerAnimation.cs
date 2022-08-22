using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region SerializeField

        [Header("Animation Flags")]
        [SerializeField] private bool _run;
        [SerializeField] private bool _jump;

        [Header("Other Components")]
        [SerializeField] private Animator _animator;
        #endregion

        #region Properties
        public bool IsRunning => _run;
        public bool IsJumping => _jump;
        public bool IsIdle => !_run && !_jump;
        #endregion
        public void StartRunning()
        {
            RunSet(1);
        }

        public void StopRunning()
        {
            RunSet(0);
        }
        private void RunSet(int running)
        {
            _animator.SetBool("run", running != 0);
            _run = running != 0;
        }
        private void JumpSet(int jumping)
        {
            _animator.SetBool("jump", jumping != 0);
            _jump = jumping != 0;
        }

    }
}