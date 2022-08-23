using Assets.MainBoard.Scripts.Route;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.MainBoard.Scripts.Player.States
{
    [System.Serializable]
    public class PlayerDeathState : PlayerBaseState
    {
        #region Private Members
        [SerializeField] private PathFinder _pathFinder;

        private Platform[] _path;
        #endregion

        #region Properties
        public PathFinder PathFinder { get => _pathFinder; set => _pathFinder = value; }
        #endregion

        public PlayerDeathState(PlayerStateContext context, PlayerData playerData, string animBoolName) : base(context, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

        }

        public void OnDeath()
        {
            Platform founded = _pathFinder.ChooseGrave();
            context.Running.CurrentPlatform = founded;
            context.gameObject.transform.position = new Vector3(founded.position.x, founded.position.y + 0.25f, founded.position.z);
        }

        // TODO: Death animasyonunun sonuna event olarak ekle...
        public override void AnimationEnded()
        {
            OnDeath();
            SwitchState(context.Land);
        }
    }
}