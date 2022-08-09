using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region SerializeField

        [Header("Animation Flags")]
        [SerializeField] private bool _run;
        [SerializeField] private bool _dead;
        [SerializeField] private bool _hit;
        [SerializeField] private bool _gettingHit;

        [Header("Other Scripts")]
        [SerializeField] private Animator _animator;
        #endregion

        #region Properties
        public bool IsRunning => _run;
        public bool IsDead => _dead;
        public bool IsIdle => !_run && !_dead;
        public bool IsGettingHit => _gettingHit;
        public bool IsHitting => _hit;
        #endregion
        public void StartRunning()
        {
            RunSet(1);
        }

        public void StopRunning()
        {
            RunSet(0);
        }

        public void StartGettingHit()
        {
            GettingHitSet(1);
        }

        public void StopGettingHit()
        {
            GettingHitSet(0);
        }

        public void StartToHit()
        {
            HitSet(1);
        }

        public void EndToHit()
        {
            HitSet(0);
        }
        private void RunSet(int running)
        {
            _animator.SetBool("running", running != 0);
            _run = running != 0;
        }

        private void GettingHitSet(int hit)
        {
            _animator.SetBool("gettinghit", hit != 0);
            _gettingHit = hit != 0;
        }

        private void HitSet(int hit)
        {
            _animator.SetBool("punch", hit != 0);
            _hit = hit != 0;
        }
    }
}