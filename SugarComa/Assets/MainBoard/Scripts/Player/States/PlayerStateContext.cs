using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Route;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerStateContext : MonoBehaviour
    {
        #region Serialize Field
        
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PathTracker _pathTracker;
        [SerializeField] private PathFinder _pathFinder;
        #endregion

        #region Private Members

        private Platform _currentPlatform;
        private PlayerBaseState _currentState;
        private PlayerStateFactory _factory;
        #endregion

        #region Properties

        public PlayerBaseState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
        public Animator Animator => _animator;
        public PathTracker PathTracker => _pathTracker;
        public PathFinder PathFinder => _pathFinder;
        public Platform Platform => _currentPlatform;
        #endregion

        private void Start()
        {
            // create factory instance.
            _factory = new PlayerStateFactory(this, _playerData);
            // set current state to landing.
            _currentState = _factory.Landing;
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }

        #region Running

        public bool NextSelectionStepPressed => _playerInput.nextSelectionStepPressed;
        #endregion

        public void AnimationStarted() { _currentState.AnimationStarted(); }
        public void AnimationEnded() { _currentState.AnimationEnded(); }
    }
}
