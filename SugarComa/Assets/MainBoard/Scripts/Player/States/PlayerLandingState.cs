namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerLandingState : PlayerBaseState
    {
        public PlayerLandingState(PlayerStateContext context, PlayerData playerData, string animBoolName) : base(context, playerData, animBoolName)
        {
        }


        // ?? We didn't call this method
        public override void AnimationEnded()
        {
            SwitchState(context.Idle);
        }
    }
}
