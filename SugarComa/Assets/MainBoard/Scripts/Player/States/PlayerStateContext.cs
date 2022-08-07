using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.States
{
    // ?? [DefaultExecutionOrder(-100)]
    public class PlayerStateContext : MonoBehaviour
    {
        #region Serialize Field
        
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private PathTracker _pathTracker;
        [SerializeField] private PathFinder _pathFinder;
        [SerializeField] Cinemachine.CinemachineBrain cinemachineBrain;
        [SerializeField] GameObject _dice;
        #endregion

        #region Private Members

        private Platform _currentPlatform;
        private PlayerInputAction _playerStateContext;
        private PlayerBaseState _currentState;
        private PlayerStateFactory _factory;

        private bool _readyToClear; // used to keep input in sync
        private bool _clearNextTick = false;
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

        /// <summary>
        /// if an animation or action will play to all players.
        /// </summary>
        public static bool canPlayersAct = true;
        public Cinemachine.CinemachineBrain CineMachineBrain { set => cinemachineBrain = value; }
        public GameObject Dice { get => _dice; }
        #endregion

        #region Move To PlayerData

        //TODO: Must move to playerData
        public bool isMyTurn;

        #endregion

        private void Awake()
        {
            // Initialize the player inputs.
            _playerStateContext = new PlayerInputAction();
            _playerStateContext.MainBoard.Space.started += OnSpace;
            _playerStateContext.MainBoard.Space.canceled += OnSpace;

            _playerStateContext.MainBoard.A.started += OnAPressed;
            _playerStateContext.MainBoard.A.canceled += OnAPressed;

            _playerStateContext.MainBoard.D.started += OnDPressed;
            _playerStateContext.MainBoard.D.canceled += OnDPressed;

            _playerStateContext.MainBoard.Return.started += OnReturnPressed;
            _playerStateContext.MainBoard.Return.canceled += OnReturnPressed;

            _playerStateContext.MainBoard.I.started += OnIPressed;
            _playerStateContext.MainBoard.I.canceled += OnIPressed;

            _playerStateContext.MainBoard.M.started += OnMPressed;
            _playerStateContext.MainBoard.M.canceled += OnMPressed;

            _playerStateContext.MainBoard.Escape.started += OnEscapePressed;
            _playerStateContext.MainBoard.Escape.canceled += OnEscapePressed;
        }

        private void Start()
        {
            // create factory instance.
            _factory = new PlayerStateFactory(this, _playerData);
            // set current state to landing.
            _currentState = _factory.Landing;
        }

        private void Update()
        {
            _clearNextTick = false;

            ClearInputs();

            if (GameManager.IsGameOver) return;
            if (isMyTurn && canPlayersAct && !cinemachineBrain.IsBlending)
            {
                // ?? In here, inputs were processing. 
                IsMouseMove();
            }
        }

        private void FixedUpdate()
        {
            _readyToClear = true;

            // make sure inputs cleared.
            // only if fixed update being called more than update.
            if (_clearNextTick)
            {
                ClearInputs();
                _clearNextTick = false;
            }
            _clearNextTick = true;

            _currentState.FixedUpdate();
        }

        #region Running
        
        private bool _spacePressed;
        public bool SpacePressed => _spacePressed;

        private void OnSpace(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _spacePressed = obj.ReadValue<bool>();
        }
        #endregion

        #region Selection
        // A
        private bool _selectLeftPressed;
        public bool SelectLeftPressed => _selectLeftPressed;

        private void OnAPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _selectLeftPressed = obj.ReadValue<bool>();
        }

        // D
        private bool _selectRightPressed;
        public bool SelectRightPressed => _selectRightPressed;

        private void OnDPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _selectRightPressed = obj.ReadValue<bool>();
        }

        // Return
        private bool _applySelectPressed;
        public bool ApplySelectPressed => _applySelectPressed;

        private void OnReturnPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _applySelectPressed = obj.ReadValue<bool>();
        }
        #endregion

        #region Inventory
        // I
        private bool _openInventory;
        public bool OpenInventory => _openInventory;

        private void OnIPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _openInventory = obj.ReadValue<bool>();
        }

        #endregion

        #region Map
        // M
        private bool _openMap;
        public bool OpenMap => _openMap;

        private void OnMPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _openMap = obj.ReadValue<bool>();
        }
        #endregion

        #region UI
        // Escape
        private bool _closeUI;
        public bool CloseUI => _closeUI;

        private void OnEscapePressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _closeUI = obj.ReadValue<bool>();
        }
        #endregion

        #region Mouse Control
        private bool _useMouseItem;
        public bool UseMouseItem
        {
            get { return _useMouseItem; }
            set { _useMouseItem = value; }
        }
        private void IsMouseMove()
        {
            _useMouseItem = (_useMouseItem || Input.GetMouseButtonDown(0)) && ItemPool._isItemUsing;
        }
        #endregion

        private void ClearInputs()
        {
            //If we're not ready to clear input, return
            if (!_readyToClear) return;

            //Reset all inputs
            _spacePressed = false;
            _selectLeftPressed = false;
            _selectRightPressed = false;
            _applySelectPressed = false;
            _openInventory = false;
            _closeUI = false;
            _openMap = false;

            _readyToClear = false;
        }

        public void AnimationStarted() { _currentState.AnimationStarted(); }
        public void AnimationEnded() { _currentState.AnimationEnded(); }

        #region Enabling / Disabling
        private void OnEnable()
        {
            _playerStateContext.Enable();
        }

        private void OnDisable()
        {
            _playerStateContext.Disable();
        }
        #endregion
    }
}
