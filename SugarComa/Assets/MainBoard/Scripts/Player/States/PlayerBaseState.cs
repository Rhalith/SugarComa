using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Route;
using UnityEngine;

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

            // Send previous and next animation state bool hash's
            SendStateData(context.CurrentState.animBoolHash, state.animBoolHash);
            // exit the current state of the context
            Exit();
            // enter the new state
            state.Enter();
            // switch the state of the context
            context.CurrentState = state;
        }

        public void SendStateData(int prevAnimBoolHash, int nextAnimBoolHash)
        {
            SteamServerManager.Instance.
                SendingMessageToAll(NetworkHelper.Serialize(new AnimationStateData
                (prevAnimBoolHash, nextAnimBoolHash, MessageType.AnimationStateUpdate)));
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