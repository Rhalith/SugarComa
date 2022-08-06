namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateContext context, PlayerData playerData, PlayerStateFactory factory, string animBoolName) : base(context, playerData, factory, animBoolName)
        {
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void CheckStateChanges()
        {
            if (context.NextSelectionStepPressed)
            {
                SwitchState(factory.Running);
            }
        }
    }
}
