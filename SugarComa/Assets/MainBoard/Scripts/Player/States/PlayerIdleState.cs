namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateContext context, PlayerData playerData, PlayerStateFactory factory, string animBoolName) : base(context, playerData, factory, animBoolName)
        {
        }

        public override void CheckStateChanges()
        {
            if (context.NextSelectionStepPressed)
            {
                SwitchState(factory.Running);
            }
        }

        public override void AnimationStarted()
        {
        }

        public override void AnimationEnded()
        {
        }
    }
}
