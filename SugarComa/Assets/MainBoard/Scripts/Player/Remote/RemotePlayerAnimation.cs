using Assets.MainBoard.Scripts.Player.Items.BoxGloves;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using System.Collections;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Remote
{
    [System.Obsolete("Bu script düzenlenip, kaldýrýlacak.")]
    public class RemotePlayerAnimation : MonoBehaviour
    {
        #region SerializeField

        [Header("Animation Flags")]
        [SerializeField] private bool _jump;
        [SerializeField] private bool _run;
        [SerializeField] private bool _land;
        [SerializeField] private bool _surprised;
        [SerializeField] private bool _dead;

        [Header("Other Scripts")]
        [SerializeField] private Animator _animator;
        [SerializeField] private RemotePlayerMovement _playerMovement;
        [SerializeField] private RemoteScriptKeeper _scriptKeeper;
        [SerializeField] private GameObject _boxGloves;
        [SerializeField] private RagdollOnOff _ragdollOnOff;
        #endregion

        #region Properties
        [SerializeField] GameObject _dice;

        public bool IsLanding => _land;
        public bool IsJumping => _jump;
        public bool IsRunning => _run;
        public bool IsSurprised => _surprised;
        public bool IsDead => _dead;
        public bool IsIdle => !_run && !_land && !_jump && !_surprised && !_dead;

        #endregion
        /// <summary>
        /// Triggers running animation.
        /// </summary>
        /// <param name="running"></param>
        private void RunSet(int running)
        {
            _animator.SetBool("running", running != 0);
            _run = running != 0;
        }

        /// <summary>
        /// Triggers jump animation.
        /// </summary>
        /// <param name="jump"></param>
        private void JumpSet(int jump)
        {
            _animator.SetBool("jump", jump != 0);
            _jump = jump != 0;
            //if (jump == 0)
            //{
            //    _dice.SetActive(false);
            //    _playerMovement.DiceText.enabled = true;
            //    IEnumerator waitForCloseText()
            //    {
            //        yield return null;
            //        yield return new WaitForSeconds(0.5f);
            //        _playerMovement.DiceText.enabled = false;
            //    }
            //    StartCoroutine(waitForCloseText());
            //}

        }

        /// <summary>
        /// Triggers landing animation.
        /// </summary>
        /// <param name="landing"></param>
        private void LandSet(int landing)
        {
            _animator.SetBool("landing", landing != 0);
            _land = landing != 0;
            //if (_playerMovement.PlayerCollector.isDead && landing == 0)
            //{
            //    DeathSet(0);
            //    _playerMovement.PlayerCollector.isDead = false;
            //    _scriptKeeper._playerCamera.Priority = 1;
            //}
        }

        /// <summary>
        /// Triggers surprise animation.
        /// </summary>
        /// <param name="surprised"></param>
        private void SurpriseSet(int surprised)
        {
            _animator.SetBool("surprised", surprised != 0);
            _surprised = surprised != 0;
        }

        /// <summary>
        /// Triggers death animation.
        /// </summary>
        /// <param name="dying"></param>
        /// TODO
        private void DeathSet(int dying)
        {
            _animator.SetBool("dead", dying != 0);
            _dead = dying != 0;
        }

        /// <summary>
        /// Jumping and rolling dice
        /// </summary>
        public void RollDice()
        {
            JumpSet(1);
        }

        /// <summary>
        /// After jumping start running
        /// </summary>
        public void StartRunning()
        {
            RunSet(1);
        }

        /// <summary>
        /// If player stops
        /// </summary>
        public void StopRunning()
        {
            RunSet(0);
        }

        /// <summary>
        /// If player is stopped in Selector and selected a way
        /// </summary>
        public void ContinueRunning()
        {
            RunSet(1);
        }
        /// <summary>
        /// When player landed.
        /// </summary>
        public void LandPlayer()
        {
            LandSet(1);
        }
        /// <summary>
        /// When death start.
        /// </summary>
        public void StartDeath()
        {
            DeathSet(1);
        }

        /// <summary>
        /// Set box gloves off, Invokes at the start of "BoxGlovesLeaving" animation.
        /// </summary>
        public void SetGlovesOff()
        {
            _boxGloves.SetActive(false);
            ItemUsing.BoxGlovesUsing = false;
            _boxGloves.GetComponent<BoxGloves>()._hitBox.SetActive(false);
        }
        /// <summary>
        /// Set box gloves on, Invokes at the end of "BoxGlovesTaking" animation.
        /// </summary>
        public void SetGlovesOn()
        {
            _boxGloves.SetActive(true);
            ItemUsing.BoxGlovesUsing = true;
            _boxGloves.GetComponent<BoxGloves>()._hitBox.SetActive(true);
        }
    }
}