using Assets.MainBoard.Scripts.Utils.InventorySystem;
using Assets.MainBoard.Scripts.Utils.PlatformUtils;
using Assets.MainBoard.Scripts.Utils.CamUtils;
using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Route;
using UnityEngine;
using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.UI;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Networking;

namespace Assets.MainBoard.Scripts.Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        #region Fields

        private RouteSelectorDirection _selectorDirection;
        #endregion

        #region Serialize Fields

        [Header("Status")]
        [SerializeField] private int _currentStep;
        public int goalStep = 5;
        public int maximumStep = 1;
        public bool isDiceRolled;
        //public bool isAnimationStopped;
        //public bool isRunningAnimation;
        public bool isUserInterfaceActive;

        [Header("Other Scripts")]
        [SerializeField] private ItemPool _itemPool;
        [SerializeField] private MapCamera _mapCamera;
        [SerializeField] private PathFinder _pathFinder;
        [SerializeField] private PathTracker _pathTracker;
        [SerializeField] private PlayerStateContext _playerStateContext;
        [SerializeField] private Platform _currentPlatform;
        [SerializeField] private GameController _gameController;
        [SerializeField] private PlayerCollector _playerCollector;
        [SerializeField] private PlayerInventory _playerInventory;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private GobletSelection _gobletSelection;
        [SerializeField] private TMPro.TMP_Text _diceText;
        [SerializeField] private GameObject _dice;
        #endregion

        #region Properties

        public int CurrentStep => _currentStep;
        public MapCamera MapCamera { set => _mapCamera = value; }
        public PathFinder PathFinder { get => _pathFinder; set => _pathFinder = value; }
        public Platform CurrentPlatform { get => _currentPlatform; set => _currentPlatform = value; }
        public GameController GameController { get => _gameController; set => _gameController = value; }
        public PlayerCollector PlayerCollector { get => _playerCollector; }

        public TMPro.TMP_Text DiceText { get => _diceText; }
        #endregion

        public PlayerIdleState(PlayerStateContext context, PlayerData playerData, PlayerStateFactory factory, string animBoolName) : base(context, playerData, factory, animBoolName)
        {
            // Burayı doldurmak lazım mı???
        }

        public override void Start()
        {
            _gameController.ChangeText();
            _gameController.ChangeInventory();
            _pathTracker.OnTrackingStarted += OnTrackingStarted;
            _pathTracker.OnCurrentPlatformChanged += OnCurrentPlatformChanged;
            _pathTracker.OnTrackingStopped += OnTrackingStopped;

            // TODO
            _gobletSelection.OnTakeIt += GobletSelection_OnTakeIt;
            _gobletSelection.OnLeaveIt += GobletSelection_OnLeaveIt;
        }

        public override void Update()
        {
            if (!isUserInterfaceActive && context.isMyTurn)
            {
                if (_currentStep <= 0 && context.SpacePressed)
                {
                    RollDice();
                }
                else if (!_currentPlatform.HasSelector && isDiceRolled && !_pathTracker.isMoving)
                {
                    StartMove();
                }
                else if (_currentStep > 0 && _currentPlatform.HasSelector)
                {
                    ProcessSelect();
                }
                ProcessUI();
            }
        }
        private void StartMove()
        {
            _currentPlatform.isPlayerInPlatform = false;
            if (context.SpacePressed || !_playerAnimation.IsJumping) // space
            {
                _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep), PlatformSpec.Goal, _currentPlatform.HasSelector);
            }
            isDiceRolled = false;
        }
        private void ProcessSelect()
        {
            var leftPlatform = _currentPlatform.selector.left;
            var rightPlatform = _currentPlatform.selector.right;
            _currentPlatform.selector.left.SetActive(true);
            _currentPlatform.selector.right.SetActive(true);

            if (context.SelectLeftPressed && leftPlatform.activeInHierarchy) 
                SelectPlatform(RouteSelectorDirection.Left);
            else if (context.SelectRightPressed && rightPlatform.activeInHierarchy) 
                SelectPlatform(RouteSelectorDirection.Right);

            if (context.ApplySelectPressed && _selectorDirection != RouteSelectorDirection.None)
            {
                RouteSelectorDirection temp = _selectorDirection;
                SelectPlatform(RouteSelectorDirection.None, leftPlatform, rightPlatform);
                _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep, temp), PlatformSpec.Goal);
            }
        }

        private void RollDice()
        {
            _currentStep = Random.Range(1, 10);
            _playerAnimation.RollDice();
            isDiceRolled = true;
            _diceText.text = _currentStep.ToString();
        }

        private void SelectPlatform(RouteSelectorDirection direction, GameObject left = null, GameObject right = null)
        {
            _selectorDirection = direction;
            _currentPlatform.SetSelectorMaterials(direction);
            if (left != null || right != null)
            {
                left.SetActive(false);
                right.SetActive(false);
            }
        }

        private void ProcessUI()
        {
            if (_playerStateContext.OpenInventory)
            {
                _playerInventory.OpenInventory();
                isUserInterfaceActive = true;
            }
            else if (_playerStateContext.CloseUI && !ItemPool._isItemUsing)
            {
                _playerInventory.CloseInventory();
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority - 1, true);
                isUserInterfaceActive = false;
            }
            else if (_playerStateContext.OpenMap)
            {
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority + 1);
                isUserInterfaceActive = true;
            }
            else if (_playerStateContext.CloseUI && ItemPool._isItemUsing)
            {
                _itemPool.CloseItem();
                isUserInterfaceActive = false;
            }
        }

        private void GobletSelection_OnLeaveIt()
        {
            _currentPlatform.isPlayerInPlatform = false;
            _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep), PlatformSpec.Goal);
            isUserInterfaceActive = false;
        }

        private void GobletSelection_OnTakeIt()
        {
            _currentPlatform.ResetSpec();
            isUserInterfaceActive = false;
            PlayerStateContext.canPlayersAct = true;
            _dice.SetActive(true);
            _currentStep = 0;
        }

        private void OnTrackingStarted()
        {
            _playerAnimation.StartRunning();
        }

        private void OnCurrentPlatformChanged()
        {
            var current = _pathTracker.CurrentPlatform;
            if (current != null)
            {
                if (_pathTracker.Next != null)
                {
                    NetworkData networkData =
                        new NetworkData(MessageType.InputDown, _pathTracker.Next.position);
                    SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(networkData));

                    _currentPlatform = current;
                    _currentStep--;
                }
                else
                {
                    _currentPlatform = current;
                    _currentStep--;
                }
            }
        }

        private void OnTrackingStopped()
        {
            _playerAnimation.StopRunning();
            _playerCollector.CheckCurrentNode(_currentPlatform);

            if (_currentPlatform.HasSelector)
            {
                if (_currentStep <= 0) _dice.SetActive(true);
            }
            else if (_currentPlatform.spec != PlatformSpec.Goal)
            {
                _dice.SetActive(true);
            }

            if (_currentStep <= 0)
            {
                _playerStateContext.isMyTurn = false;
                _playerStateContext.Dice.SetActive(false);
                SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new TurnNetworkData((byte)NetworkManager.Instance.Index, MessageType.TurnOver)));
            }
        }

        /* TODO: OnDeath nerede olcak...
        public void OnDeath()
        {
            Platform founded = _pathFinder.ChooseGrave();
            _currentPlatform = founded;
            gameObject.transform.position = new Vector3(founded.position.x, founded.position.y + 0.25f, founded.position.z);
        }
        */

        public override void CheckStateChanges()
        {
            // There is a jump state
            // at first, we must check that state ended.
            if (context.SpacePressed)
            {
                SwitchState(factory.Running);
            }
        }

        public override void AnimationStarted()
        {
        }

        public override void AnimationEnded()
        {
        }
    }
}
