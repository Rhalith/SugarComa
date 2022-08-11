using Assets.MainBoard.Scripts.Route;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.States
{
    public abstract class PlayerBaseState
    {
        private PlayerStateContext _context;
        private PlayerData _playerData;
        private string _animBoolName;
        private int _animBoolHash;
        private bool _sendData;

        protected PlayerStateContext Context => _context;
        protected PlayerData PlayerData => _playerData;
        protected string AnimBoolName => _animBoolName;
        protected int AnimBoolHash => _animBoolHash;
        protected bool SendData => _sendData;

        public PlayerBaseState() { }

        public PlayerBaseState(PlayerStateContext context, PlayerData playerData, string animBoolName, bool sendData = true)
        {
            Initialize(context, playerData, animBoolName, sendData);
        }

        public void Initialize(PlayerStateContext context, PlayerData playerData, string animBoolName, bool sendData = true)
        {
            _context = context;
            _playerData = playerData;
            _animBoolName = animBoolName;
            _sendData = sendData;
            _animBoolHash = Animator.StringToHash(animBoolName);
        }

        public void SwitchState(PlayerBaseState state)
        {
            if (Context.CurrentState == state) return;
            // exit the current state of the context
            Exit();
            // enter the new state
            state.Enter();
            // switch the state of the context
            Context.CurrentState = state;
        }

        public virtual void Enter()
        {
            Context.Animator.SetBool(AnimBoolHash, true);
            if (SendData)
            {
                
            }
        }
        
        public virtual void Exit()
        {
            Context.Animator.SetBool(AnimBoolHash, false);
        }

        public virtual void Update()
        {
            CheckStateChanges();
        }

        public virtual void FixedUpdate()
        {
            CheckPhysicsStateChanges();
        }

        public virtual void CheckPhysicsStateChanges() { }
        public virtual void CheckStateChanges() { }

        public virtual void AnimationStarted() { }

        public virtual void AnimationEnded() 
        {

        }
    }
}