namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerLandingState : PlayerBaseState
    {
        public PlayerLandingState(PlayerStateContext context, PlayerData playerData, PlayerStateFactory factory, string animBoolName) : base(context, playerData, factory, animBoolName)
        {
        }


        // ?? We didn't call this method
        public override void AnimationEnded()
        {
            SwitchState(factory.Idle);
        }
    }
}
