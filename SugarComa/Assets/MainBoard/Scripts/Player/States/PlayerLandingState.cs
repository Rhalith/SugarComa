namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerLandingState : PlayerBaseState
    {
        public PlayerLandingState(PlayerStateContext context, PlayerData playerData, PlayerStateFactory factory, string animBoolName) : base(context, playerData, factory, animBoolName)
        {
        }

        public override void AnimationEnded()
        {
            SwitchState(factory.Idle);
        }
    }
}
