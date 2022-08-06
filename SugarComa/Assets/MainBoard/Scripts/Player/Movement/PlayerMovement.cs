using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.UI;
using Assets.MainBoard.Scripts.Utils.CamUtils;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using Assets.MainBoard.Scripts.Utils.PlatformUtils;
using System.Collections;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Movement
{
    public class PlayerMovement : MonoBehaviour
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
        [SerializeField] private PlayerInput _playerInput;
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

        private void Start()
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

        private void Update()
        {
            if (!isUserInterfaceActive && _playerAnimation.IsIdle && _playerInput.isMyTurn)
            {
                if (_currentStep <= 0 && _playerInput.nextSelectionStepPressed)
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
            }
            ProcessUI();
        }

        private void StartMove()
        {
            _currentPlatform.isPlayerInPlatform = false;
            if (_playerInput.nextSelectionStepPressed || !_playerAnimation.IsJumping) // space
            {
                _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep), PlatformSpec.Goal, _currentPlatform.HasSelector);
            }
            else if (_playerInput.nextSelectionPressed) // X
            {
                _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform), PlatformSpec.Goal, _currentPlatform.HasSelector);

            }
            else if (_playerInput.nextGoalPressed) // C
            {
                _pathTracker.StartTracking(_pathFinder.FindBest(_currentPlatform, PlatformSpec.Goal), PlatformSpec.Goal, _currentPlatform.HasSelector);
            }
            else if (_playerInput.nextGoalStepPressed) // V
            {
                _pathTracker.StartTracking(_pathFinder.FindBest(_currentPlatform, PlatformSpec.Goal, goalStep), PlatformSpec.Goal, _currentPlatform.HasSelector);
            }
            else if (_playerInput.moveToBackStepPressed) // B
            {
                _pathTracker.RestartTracking(maximumStep, false, PlatformSpec.Goal);
            }
            isDiceRolled = false;
        }


        private void ProcessUI()
        {
            if (_playerInput.openInventory)
            {
                _playerInventory.OpenInventory();
                isUserInterfaceActive = true;
            }
            else if (_playerInput.closeUI && !ItemPool._isItemUsing)
            {
                _playerInventory.CloseInventory();
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority - 1, true);
                isUserInterfaceActive = false;
            }
            else if (_playerInput.openMap)
            {
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority + 1);
                isUserInterfaceActive = true;
            }
            else if (_playerInput.closeUI && ItemPool._isItemUsing)
            {
                _itemPool.CloseItem();
                isUserInterfaceActive = false;
            }
        }

        private void ProcessSelect()
        {
            var leftPlatform = _currentPlatform.selector.left;
            var rightPlatform = _currentPlatform.selector.right;
            _currentPlatform.selector.left.SetActive(true);
            _currentPlatform.selector.right.SetActive(true);

            if (_playerInput.selectLeftPressed && leftPlatform.activeInHierarchy) SelectPlatform(RouteSelectorDirection.Left);
            else if (_playerInput.selectRightPressed && rightPlatform.activeInHierarchy) SelectPlatform(RouteSelectorDirection.Right);

            if (_playerInput.applySelectPressed && _selectorDirection != RouteSelectorDirection.None)
            {
                RouteSelectorDirection temp = _selectorDirection;
                SelectPlatform(RouteSelectorDirection.None, leftPlatform, rightPlatform);
                _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep, temp), PlatformSpec.Goal);
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
            PlayerInput.canPlayersAct = true;
            _dice.SetActive(true);
            _currentStep = 0;
            //TODO
            //if (_currentStep > 0)
            //{
            //    _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep), PlatformSpec.Goal);
            //}
            //else
            //{
            //    _dice.SetActive(true);
            //}
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
                _playerInput.isMyTurn = false;
                _playerInput.Dice.SetActive(false);
                SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new TurnNetworkData((byte)NetworkManager.Instance.Index, MessageType.TurnOver)));
            }
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

        private void RollDice()
        {
            _currentStep = Random.Range(1, 10);
            _playerAnimation.RollDice();
            isDiceRolled = true;
            _diceText.text = _currentStep.ToString();
        }

        public void OnDeath()
        {
            Platform founded = _pathFinder.ChooseGrave();
            _currentPlatform = founded;
            gameObject.transform.position = new Vector3(founded.position.x, founded.position.y + 0.25f, founded.position.z);
        }
    }
}