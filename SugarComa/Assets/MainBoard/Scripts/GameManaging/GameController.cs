using UnityEngine;
using Assets.MainBoard.Scripts.UI;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Utils.InventorySystem;

namespace Assets.MainBoard.Scripts.GameManaging
{
    public class GameController : MonoBehaviour
    {
        #region Public Fiels
        public PlayerHandler _playerHandler;
        #endregion

        #region Readonly Fiels
        readonly NotifyScript notifyScript = new();
        #endregion

        #region Private Fiels
        private TextChanger _textChanger;
        private InventoryChanger _inventoryChanger1, _inventoryChanger2, _inventoryChanger3, _inventoryChanger4, _inventoryChanger5, _inventoryChanger6, _inventoryChanger7, _inventoryChanger8, _inventoryChanger9, _inventoryChanger10;
        #endregion

        /// <summary>
        /// It notifies to UI for change. If you want to specify the UI you can add ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        public void ChangeText(RemoteScriptKeeper remoteKeeper = null, ScriptKeeper keeper = null)
        {
            if (keeper != null)
            {
                InstanceUIElements(null, keeper);
            }
            else if(remoteKeeper != null)
            {
                InstanceUIElements(remoteKeeper);
            }
            else
            {
                InstanceUIElements();
            }
            notifyScript.Notify();
            ClearUIObserver();
        }

        /// <summary>
        /// It notifies to Inventory UI for change. If you want to specify the Inventory UI you can add ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        public void ChangeInventory(ScriptKeeper keeper = null)
        {
            if (keeper != null)
            {
                InstanceInventoryObjects(keeper);
                notifyScript.NotifyInventory();
                ClearInventoryObserver();
            }
            else
            {
                InstanceInventoryObjects();
                notifyScript.NotifyInventory();
                ClearInventoryObserver();
            }
        }

        /// <summary>
        /// It instances UI elements for notify. If you want to specify the UI you can add ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        private void InstanceUIElements(RemoteScriptKeeper remoteKeeper = null, ScriptKeeper keeper = null)
        {
            if (keeper != null)
            {
                _textChanger = new(keeper.playerGold, keeper.playerHealth, keeper.playerGoblet, keeper.playerCollector);
            }
            else if (remoteKeeper != null)
            {
                _textChanger = new(remoteKeeper.playerGold, remoteKeeper.playerHealth, remoteKeeper.playerGoblet, remoteKeeper.playerCollector);
            }
            else
            {
                _textChanger = new(_playerHandler.mainplayerGold, _playerHandler.mainplayerHealth, _playerHandler.mainplayerGoblet, _playerHandler.mainPlayerCollector);
            }
            notifyScript.AddObserver(_textChanger);
        }

        /// <summary>
        /// It instances Inventory UI elements for notify. If you want to specify the Inventory UI you can add ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        private void InstanceInventoryObjects(ScriptKeeper keeper = null)
        {
            if (keeper != null)
            {
                _inventoryChanger1 = new(keeper.playerInventory._items[0]); AddToObserver(_inventoryChanger1);
                _inventoryChanger2 = new(keeper.playerInventory._items[1]); AddToObserver(_inventoryChanger2);
                _inventoryChanger3 = new(keeper.playerInventory._items[2]); AddToObserver(_inventoryChanger3);
                _inventoryChanger4 = new(keeper.playerInventory._items[3]); AddToObserver(_inventoryChanger4);
                _inventoryChanger5 = new(keeper.playerInventory._items[4]); AddToObserver(_inventoryChanger5);
                _inventoryChanger6 = new(keeper.playerInventory._items[5]); AddToObserver(_inventoryChanger6);
                _inventoryChanger7 = new(keeper.playerInventory._items[6]); AddToObserver(_inventoryChanger7);
                _inventoryChanger8 = new(keeper.playerInventory._items[7]); AddToObserver(_inventoryChanger8);
                _inventoryChanger9 = new(keeper.playerInventory._items[8]); AddToObserver(_inventoryChanger9);
                _inventoryChanger10 = new(keeper.playerInventory._items[9]); AddToObserver(_inventoryChanger10);
            }
            else
            {
                _inventoryChanger1 = new(_playerHandler.mainPlayerInventory._items[0]); AddToObserver(_inventoryChanger1);
                _inventoryChanger2 = new(_playerHandler.mainPlayerInventory._items[1]); AddToObserver(_inventoryChanger2);
                _inventoryChanger3 = new(_playerHandler.mainPlayerInventory._items[2]); AddToObserver(_inventoryChanger3);
                _inventoryChanger4 = new(_playerHandler.mainPlayerInventory._items[3]); AddToObserver(_inventoryChanger4);
                _inventoryChanger5 = new(_playerHandler.mainPlayerInventory._items[4]); AddToObserver(_inventoryChanger5);
                _inventoryChanger6 = new(_playerHandler.mainPlayerInventory._items[5]); AddToObserver(_inventoryChanger6);
                _inventoryChanger7 = new(_playerHandler.mainPlayerInventory._items[6]); AddToObserver(_inventoryChanger7);
                _inventoryChanger8 = new(_playerHandler.mainPlayerInventory._items[7]); AddToObserver(_inventoryChanger8);
                _inventoryChanger9 = new(_playerHandler.mainPlayerInventory._items[8]); AddToObserver(_inventoryChanger9);
                _inventoryChanger10 = new(_playerHandler.mainPlayerInventory._items[9]); AddToObserver(_inventoryChanger10);
            }

        }

        /// <summary>
        /// Clears the UI Observer for another call.
        /// </summary>
        private void ClearUIObserver()
        {
            notifyScript.RemoveObserver(_textChanger);
        }

        /// <summary>
        /// Clears the Inventory UI Observer for another call.
        /// </summary>
        private void ClearInventoryObserver()
        {
            RemoveFromObserver(_inventoryChanger1);
            RemoveFromObserver(_inventoryChanger2);
            RemoveFromObserver(_inventoryChanger3);
            RemoveFromObserver(_inventoryChanger4);
            RemoveFromObserver(_inventoryChanger5);
            RemoveFromObserver(_inventoryChanger6);
            RemoveFromObserver(_inventoryChanger7);
            RemoveFromObserver(_inventoryChanger8);
            RemoveFromObserver(_inventoryChanger9);
            RemoveFromObserver(_inventoryChanger10);
        }

        /// <summary>
        /// Adds Inventory element to Observer.
        /// </summary>
        /// <param name="invchanger"></param>
        private void AddToObserver(InventoryChanger invchanger)
        {
            notifyScript.AddInventoryObserver(invchanger);
        }

        /// <summary>
        /// Removes Inventory element to Observer.
        /// </summary>
        /// <param name="invchanger"></param>
        private void RemoveFromObserver(InventoryChanger invchanger)
        {
            notifyScript.RemoveInventoryObserver(invchanger);
        }
    }
}