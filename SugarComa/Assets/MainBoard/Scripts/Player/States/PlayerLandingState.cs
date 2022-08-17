using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Route;
using UnityEngine;
using System.Linq;

namespace Assets.MainBoard.Scripts.Player.States
{
    [System.Serializable]
    public class PlayerLandingState : PlayerBaseState
    {
        #region Properties
        public GoalSelector GoalSelector { get; set; }
        #endregion

        public PlayerLandingState(PlayerStateContext context, PlayerData playerData, string animBoolName) : base(context, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (NetworkManager.Instance.playerList.ElementAt(0).Key == SteamManager.Instance.PlayerSteamId && !GoalSelector.isAnyGoalPlatform)
            {
                GoalSelector.RandomGoalSelect();
            }
        }

        public override void AnimationEnded()
        {
            SwitchState(context.Idle);
        }
    }
}
