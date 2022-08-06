using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Utils.PlatformUtils;

namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerRunningState : PlayerBaseState
    {
        private int _currentStep;

        public PlayerRunningState(PlayerStateContext context, PlayerData playerData, PlayerStateFactory factory, string animBoolName) : base(context, playerData, factory, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}
