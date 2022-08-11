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
using Assets.MainBoard.Scripts.Player.States.SubStates;

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
            // TODO
            context.GobletSelection.OnTakeIt += GobletSelection_OnTakeIt;
            context.GobletSelection.OnLeaveIt += GobletSelection_OnLeaveIt;
        }

        public override void Enter()
        {
            base.Enter();
            _currentStep = Context.Running.CurrentStep;
            _currentPlatform = Context.Running.CurrentPlatform;

            if(_currentStep == 0)
            {
                _dice.Enter();
            }
    }

        public override void Update()
        {
            ProcessUI();
            base.Update();
        }
        private void ProcessSelect()
        {
            var leftPlatform = _currentPlatform.selector.left;
            var rightPlatform = _currentPlatform.selector.right;
            _currentPlatform.selector.left.SetActive(true);
            _currentPlatform.selector.right.SetActive(true);

            if (Context.SelectLeftPressed && leftPlatform.activeInHierarchy) 
                SelectPlatform(RouteSelectorDirection.Left);
            else if (Context.SelectRightPressed && rightPlatform.activeInHierarchy) 
                SelectPlatform(RouteSelectorDirection.Right);

            if (Context.ApplySelectPressed && _selectorDirection != RouteSelectorDirection.None)
            {
                RouteSelectorDirection temp = _selectorDirection;
                SelectPlatform(RouteSelectorDirection.None, leftPlatform, rightPlatform);
                Context.Running.SelectorDir = temp;

                SwitchState(Context.Running);
                Context.Running.SelectorDir = RouteSelectorDirection.None;
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
            if (Context.OpenInventory)
            {
                _playerInventory.OpenInventory();
            }
            else if (Context.CloseUI && !ItemPool._isItemUsing)
            {
                _playerInventory.CloseInventory();
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority - 1, true);
            }
            else if (Context.OpenMap)
            {
                _mapCamera.SetCameraPriority(_mapCamera.cam, _mapCamera.mainCamera.Priority + 1);
            }
            else if (Context.CloseUI && ItemPool._isItemUsing)
            {
                _itemPool.CloseItem();
            }
        }
        
        #region Goblet Selectio events
        private void GobletSelection_OnLeaveIt()
        {
            SwitchState(Context.Running);
        }

        private void GobletSelection_OnTakeIt()
        {
            _currentPlatform.ResetSpec();
            PlayerStateContext.canPlayersAct = true;
            // TODO: Dice state olabilir.
            Context.Running.CurrentStep = 0;
        }
        #endregion

        public override void CheckStateChanges()
        {
            if (_currentStep > 0 && _currentPlatform.HasSelector)
            {
                ProcessSelect();
            }
            else if (Context.SpacePressed)
            {
                if (_currentStep <= 0)
                {
                    _dice.RollDice();
                    _currentStep = Context.Running.CurrentStep;
                    _dice.Exit();
                }
                else
                {
                    SwitchState(Context.Running);
                }
            }
        }
    }
}
