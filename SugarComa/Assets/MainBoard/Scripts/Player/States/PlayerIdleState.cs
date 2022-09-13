using Assets.MainBoard.Scripts.Player.States.SubStates;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Utils.CamUtils;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Route;
using UnityEngine;
using Assets.MainBoard.Scripts.Player.Handlers;

namespace Assets.MainBoard.Scripts.Player.States
{
    [System.Serializable]
    public class PlayerIdleState : PlayerBaseState
    {
        #region Fields
        #region SerializeField
        [SerializeField] private ItemPool _itemPool;
        [SerializeField] private MapCamera _mapCamera;
        [SerializeField] private PlayerInventory _playerInventory;
        #endregion

        #region Private Field
        private int _currentStep;
        private Platform _currentPlatform;
        private RouteSelectorDirection _selectorDirection;
        #endregion
        #endregion

        #region Properties
        public MapCamera MapCamera { set => _mapCamera = value; }
        #endregion

        #region SubStates
        [SerializeField] private PlayerDiceState _dice;
        public PlayerDiceState Dice => _dice;
        #endregion

        public PlayerIdleState(PlayerStateContext context, PlayerData playerData, string animBoolName) : base(context, playerData, animBoolName)
        {
        }

        public override void Initialize(PlayerStateContext context, PlayerData playerData, string animBoolName, bool sendData = true)
        {
            base.Initialize(context, playerData, animBoolName);

            context.GobletSelection.OnTakeIt += GobletSelection_OnTakeIt;
            context.GobletSelection.OnLeaveIt += GobletSelection_OnLeaveIt;
        }

        public override void Enter()
        {
            base.Enter();
            _currentStep = context.Running.CurrentStep;
            _currentPlatform = context.Running.CurrentPlatform;

            CheckTurnForDice();
        }

        public override void Update()
        {
            ProcessUI();
            base.Update();
        }

        public void CheckTurnForDice()
        {
            if (context.IsMyTurn && _currentStep == 0)
            {
                _dice.Enter();
            }
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
                context.Running.SelectorDir = temp;

                SwitchState(context.Running);
                context.Running.SelectorDir = RouteSelectorDirection.None;
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

        private void ProcessUI()
        {
            if (context.OpenInventory)
            {
                _playerInventory.OpenInventory();
            }
            else if (context.CloseUI && !ItemPool._isItemUsing)
            {
                _playerInventory.CloseInventory();
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority - 1, true);
            }
            else if (context.OpenMap)
            {
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority + 1);
            }
            else if (context.CloseUI && ItemPool._isItemUsing)
            {
                _itemPool.CloseItem();
            }
        }
        
        #region Goblet Selection events
        private void GobletSelection_OnLeaveIt()
        {
            if (context.Running.CurrentStep == 0)
            {
                // Bunları genel bir dosyada metoda al
                PlayerTurnHandler.NextPlayer();
                SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new TurnNetworkData(MessageType.TurnOver)));
            }
            else
                SwitchState(context.Running);
        }

        private void GobletSelection_OnTakeIt()
        {
            _currentPlatform.ResetSpec();
            PlayerStateContext.canPlayersAct = true;
            context.Running.CurrentStep = 0;
            _currentStep = 0;

            // Turn Over
            context.IsMyTurn = false;
            PlayerTurnHandler.NextPlayer();
            SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new TurnNetworkData(MessageType.TurnOver)));
        }
        #endregion

        public override void CheckStateChanges()
        {
            if (_currentStep > 0 && _currentPlatform.HasSelector)
            {
                ProcessSelect();
            }
            else if (context.SpacePressed)
            {
                if (_currentStep <= 0)
                {
                    _dice.RollDice();
                    _currentStep = context.Running.CurrentStep;
                    _dice.Exit();
                    SwitchState(context.Running);
                }
            }
        }
    }
}
