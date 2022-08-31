using UnityEngine;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;

namespace Assets.MainBoard.Scripts.Player.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateContext context;
        protected PlayerData playerData;
        protected string animBoolName;
        protected int animBoolHash;
        protected bool sendData;

        public PlayerBaseState() { }

        public PlayerBaseState(PlayerStateContext context, PlayerData playerData, string animBoolName, bool sendData = true)
        {
            Initialize(context, playerData, animBoolName, sendData);
        }

        /// <summary>
        /// This method should be called in scripts from which it is inherited.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="playerData"></param>
        /// <param name="animBoolName"></param>
        /// <param name="sendData"></param>
        public virtual void Initialize(PlayerStateContext context, PlayerData playerData, string animBoolName, bool sendData = true)
        {
            this.context = context;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
            this.sendData = sendData;
            this.animBoolHash = Animator.StringToHash(animBoolName);
        }

        public void SwitchState(PlayerBaseState state)
        {
            if (context.CurrentState == state) return;

            // exit the current state of the context
            Exit();
            // switch the state of the context
            context.CurrentState = state;
            // enter the new state
            state.Enter();
        }

        public virtual void Enter()
        {
            context.Animator.SetBool(animBoolHash, true);
            if (sendData)
            {
                SteamServerManager.Instance.
                    SendingMessageToAll(NetworkHelper.Serialize(new AnimationStateData(animBoolHash)));
            }
        }
        
        public virtual void Exit()
        {
            context.Animator.SetBool(animBoolHash, false);
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