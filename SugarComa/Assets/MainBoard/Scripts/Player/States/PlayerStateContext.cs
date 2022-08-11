using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.UI;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.States
{
    // ?? [DefaultExecutionOrder(-100)]
    public class PlayerStateContext : MonoBehaviour
    {
        #region Serialize Field

        [SerializeField] private PlayerCollector _playerCollector;
        //[SerializeField] private GameController _gameController;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private GobletSelection _gobletSelection;
        //[SerializeField] private Cinemachine.CinemachineBrain _cinemachineBrain;
        #endregion

        #region Private Members

        private PlayerInputAction _playerInput;
        private PlayerBaseState _currentState;
        #endregion

        #region Properties

        public PlayerBaseState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
        public Animator Animator => _animator;

        /// <summary>
        /// if an animation or action will play to all players.
        /// </summary>
        public static bool canPlayersAct = true;
        //public Cinemachine.CinemachineBrain CineMachineBrain { set => _cinemachineBrain = value; }
        //public GameController GameController { get => _gameController; set => _gameController = value; }
        public PlayerCollector PlayerCollector => _playerCollector;
        public GobletSelection GobletSelection => _gobletSelection;
        #endregion

        #region Move To PlayerData

        //TODO: Must move to playerData
        private bool _isMyTurn;
        public bool IsMyTurn { get => _isMyTurn; set { _isMyTurn = enabled = value; } }

        #endregion

        private void Awake()
        {
            // Initialize the player inputs.
            InitializePlayerInputs();
            InitializeStates();
        }

        private void Start()
        {
            _currentState.Enter();
            //_gameController.ChangeText();
            //_gameController.ChangeInventory();
        }

        private void Update()
        {
            if (GameManager.IsGameOver) return;

            /* TODO
            if (isMyTurn && canPlayersAct && !cinemachineBrain.IsBlending)
            {
            }
            */
            _currentState.Update();
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }

        private void InitializePlayerInputs()
        {
            _playerInput = new PlayerInputAction();
            _playerInput.MainBoard.Space1.started += OnSpace;
            _playerInput.MainBoard.Space1.canceled += OnSpace;

            _playerInput.MainBoard.A.started += OnAPressed;
            _playerInput.MainBoard.A.canceled += OnAPressed;

            _playerInput.MainBoard.D.started += OnDPressed;
            _playerInput.MainBoard.D.canceled += OnDPressed;

            _playerInput.MainBoard.Return.started += OnReturnPressed;
            _playerInput.MainBoard.Return.canceled += OnReturnPressed;

            _playerInput.MainBoard.I.started += OnIPressed;
            _playerInput.MainBoard.I.canceled += OnIPressed;

            _playerInput.MainBoard.M.started += OnMPressed;
            _playerInput.MainBoard.M.canceled += OnMPressed;

            _playerInput.MainBoard.Escape.started += OnEscapePressed;
            _playerInput.MainBoard.Escape.canceled += OnEscapePressed;

            _playerInput.MainBoard.Mouse.started += OnLeftMouseDown;
            _playerInput.MainBoard.Mouse.canceled += OnLeftMouseDown;
        }

        #region States
        [SerializeField] private PlayerIdleState _playerIdle;
        [SerializeField] private PlayerRunningState _playerRunning;
        [SerializeField] private PlayerLandingState _playerLanding;
        public PlayerIdleState Idle => _playerIdle;
        public PlayerRunningState Running => _playerRunning;
        public PlayerLandingState Land => _playerLanding;

        private void InitializeStates()
        {
            _playerIdle.Initialize(this, _playerData, "idle");
            _playerIdle.Dice.Initialize(this, _playerData, "");
            _playerLanding.Initialize(this, _playerData, "landing");
            _playerRunning.Initialize(this, _playerData, "running");

            // set current state to landing.
            _currentState = _playerLanding;
        }
        #endregion

        #region Running

        private bool _spacePressed;
        public bool SpacePressed => _spacePressed;

        private void OnSpace(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _spacePressed = obj.ReadValueAsButton();
        }
        #endregion

        #region Selection
        // A
        private bool _selectLeftPressed;
        public bool SelectLeftPressed => _selectLeftPressed;

        private void OnAPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _selectLeftPressed = obj.ReadValueAsButton();
        }

        // D
        private bool _selectRightPressed;
        public bool SelectRightPressed => _selectRightPressed;

        private void OnDPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _selectRightPressed = obj.ReadValueAsButton();
        }

        // Return
        private bool _applySelectPressed;
        public bool ApplySelectPressed => _applySelectPressed;

        private void OnReturnPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _applySelectPressed = obj.ReadValueAsButton();
        }
        #endregion

        #region Inventory
        // I
        private bool _openInventory;
        public bool OpenInventory => _openInventory;

        private void OnIPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _openInventory = obj.ReadValueAsButton();
        }

        #endregion

        #region Map
        // M
        private bool _openMap;
        public bool OpenMap => _openMap;

        private void OnMPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _openMap = obj.ReadValueAsButton();
        }
        #endregion

        #region UI
        // Escape
        private bool _closeUI;
        public bool CloseUI => _closeUI;

        private void OnEscapePressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _closeUI = obj.ReadValueAsButton();
        }
        #endregion

        #region Mouse Control
        private bool _useMouseItem;
        public bool UseMouseItem
        {
            get { return _useMouseItem; }
            set { _useMouseItem = value; }
        }
        private void OnLeftMouseDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _closeUI = obj.ReadValueAsButton();
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
            _playerIdle.Dice.Exit();
        }
        #endregion
    }
}
