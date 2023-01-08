using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region SerializeField
        [Header("Animation Flags")]
        [SerializeField] private bool _aim;
        [SerializeField] private bool _throw;
        [SerializeField] private bool _waiting;


        [Header("Other Scripts")]
        [SerializeField] private Animator _animator;
        #endregion

        #region Properties
        public bool IsWaiting => _waiting;
        public bool IsAiming => _aim;
        public bool IsThrowed => _throw;
        #endregion

        public void Aim()
        {
            AimSet(1);
        }
        public void StopAim()
        {
            AimSet(0);
        }

        public void Throw()
        {
            ThrowSet(1);
        }
        public void ThrowFalse()
        {
            ThrowSet(0);
        }

        public void Waiting()
        {
            WaitingSet(1);
        }
        public void NotWaiting()
        {
            WaitingSet(0);
        }

        private void AimSet(int aiming)
        {
            _animator.SetBool("running", aiming != 0);
            _aim = aiming != 0;
        }
        private void WaitingSet(int waiting)
        {
            _animator.SetBool("running", waiting != 0);
            _waiting = waiting != 0;
        }
        private void ThrowSet(int throws)
        {
            _animator.SetBool("running", throws != 0);
            _throw = throws != 0;
        }


    }
}
