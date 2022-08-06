namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerStateFactory
    {
        private readonly PlayerBaseState _idle;
        private readonly PlayerBaseState _landing;
        private readonly PlayerBaseState _running;

        public PlayerBaseState Idle => _idle;
        public PlayerBaseState Landing => _landing;
        public PlayerBaseState Running => _running;
        
        public PlayerStateFactory(PlayerStateContext context, PlayerData playerData)
        {
            _idle = new PlayerIdleState(context, playerData, this, "idle");
            _landing = new PlayerLandingState(context, playerData, this, "landing");
            _running = new PlayerRunningState(context, playerData, this, "running");
        }
    }
}
