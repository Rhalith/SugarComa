using Assets.MainBoard.Scripts.Route;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerStateContext : MonoBehaviour
    {
        #region Serialize Field
        
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PathTracker _pathTracker;
        [SerializeField] private PathFinder _pathFinder;
        #endregion

        #region Private Members

        private Platform _currentPlatform;
        private PlayerInputAction _playerInput;
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
        public Platform CurrentPlatform { get => _currentPlatform; set => _currentPlatform = value; }
        #endregion

        private void Awake()
        {
            // Initialize the player inputs.
            _playerInput = new PlayerInputAction();
            _playerInput.MainBoard.Space.started += OnSpace;
            _playerInput.MainBoard.Space.canceled += OnSpace;
        }

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
        
        private bool _spacePressed;
        public bool NextSelectionStepPressed => _spacePressed;

        private void OnSpace(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _spacePressed = obj.ReadValue<bool>();
        }
        #endregion

        public void AnimationStarted() { _currentState.AnimationStarted(); }
        public void AnimationEnded() { _currentState.AnimationEnded(); }

        #region Enabling / Disabling
        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }
        #endregion
    }
}
