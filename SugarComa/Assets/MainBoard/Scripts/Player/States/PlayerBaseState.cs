using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.States
{
    public abstract class PlayerBaseState
    {
        protected readonly PlayerStateContext context;
        protected readonly PlayerData playerData;
        protected readonly PlayerStateFactory factory;
        protected readonly string animBoolName;
        protected readonly int animBoolHash;
        protected bool sendData;
        
        public PlayerBaseState(PlayerStateContext context, PlayerData playerData, PlayerStateFactory factory, string animBoolName, bool sendData = true)
        {
            this.context = context;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
            this.factory = factory;
            this.sendData = sendData;
            animBoolHash = Animator.StringToHash(animBoolName);
        }
        
        public void SwitchState(PlayerBaseState state)
        {
            if (context.CurrentState == state) return;
            // exit the current state of the context
            Exit();
            // enter the new state
            state.Enter();
            // switch the state of the context
            context.CurrentState = state;
        }

        public virtual void Enter()
        {
            context.Animator.SetBool(animBoolHash, true);
            if (sendData)
            {
                
            }
        }
        
        public virtual void Exit()
        {
            context.Animator.SetBool(animBoolHash, false);
        }

        public virtual void FixedUpdate()
        {
            CheckStateChanges();
        }
        
        public virtual void CheckStateChanges() { }

        public virtual void AnimationStarted() { }
        public virtual void AnimationEnded() { }
    }
}