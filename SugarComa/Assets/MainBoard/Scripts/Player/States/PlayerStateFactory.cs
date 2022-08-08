namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerStateFactory
    {
        private readonly PlayerBaseState _idle;
        private readonly PlayerBaseState _landing;
        private readonly PlayerBaseState _running;
        private readonly PlayerBaseState _jump;
        private readonly PlayerBaseState _dead;
        private readonly PlayerBaseState _itemUsing;
        private readonly PlayerBaseState _boxing;

        public PlayerBaseState Idle => _idle;
        public PlayerBaseState Landing => _landing;
        public PlayerBaseState Running => _running;
        public PlayerBaseState Jump => _jump;
        public PlayerBaseState Dead => _dead;
        public PlayerBaseState ItemUsing => _itemUsing;
        public PlayerBaseState Boxing => _boxing;
        
        public PlayerStateFactory(PlayerStateContext context, PlayerData playerData)
        {
            _idle = new PlayerIdleState(context, playerData, this, "idle");
            _landing = new PlayerLandingState(context, playerData, this, "landing");
            _running = new PlayerRunningState(context, playerData, this, "running");

            // TODO: create state classes for these ones
            _jump = new PlayerRunningState(context, playerData, this, "jump");
            _dead = new PlayerRunningState(context, playerData, this, "dead");
            _itemUsing = new PlayerRunningState(context, playerData, this, "itemUsing");
            //_boxing = new PlayerRunningState(context, playerData, this, "boxing");
        }
    }
}
