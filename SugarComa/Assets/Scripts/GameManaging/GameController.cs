using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public PlayerHandler _playerHandler;

    readonly NotifyScript notifyScript = new();

    private TextChanger _textChanger;
    private InventoryChanger _inventoryChanger1, _inventoryChanger2, _inventoryChanger3, _inventoryChanger4, _inventoryChanger5, _inventoryChanger6, _inventoryChanger7, _inventoryChanger8, _inventoryChanger9, _inventoryChanger10;

    public void ChangeText(ScriptKeeper keeper = null)
    {
        if(keeper != null)
        {
            InstanceUIElements(keeper);
            notifyScript.Notify();
            ClearUIObserver();
        }
        else
        {
            InstanceUIElements();
            notifyScript.Notify();
            ClearUIObserver();
        }
    }

    public void ChangeInventory()
    {
        InstanceInventoryObjects();
        notifyScript.NotifyInventory();
        ClearInventoryObserver();
    }
    
    private void InstanceUIElements(ScriptKeeper keeper = null)
    {
        if(keeper != null)
        {
            _textChanger = new(keeper.playerGold, keeper.playerHealth, keeper.playerGoblet, keeper._playerCollector);
        }
        else
        {
            _textChanger = new(_playerHandler.currentplayerGold, _playerHandler.currentplayerHealth, _playerHandler.currentplayerGoblet, _playerHandler.currentPlayerCollector);
        }
        notifyScript.AddObserver(_textChanger);
    }
    private void InstanceInventoryObjects()
    {
        _inventoryChanger1 = new(_playerHandler.currentPlayerInventory._items[0]); AddToObserver(_inventoryChanger1);
        _inventoryChanger2 = new(_playerHandler.currentPlayerInventory._items[1]); AddToObserver(_inventoryChanger2);
        _inventoryChanger3 = new(_playerHandler.currentPlayerInventory._items[2]); AddToObserver(_inventoryChanger3);
        _inventoryChanger4 = new(_playerHandler.currentPlayerInventory._items[3]); AddToObserver(_inventoryChanger4);
        _inventoryChanger5 = new(_playerHandler.currentPlayerInventory._items[4]); AddToObserver(_inventoryChanger5);
        _inventoryChanger6 = new(_playerHandler.currentPlayerInventory._items[5]); AddToObserver(_inventoryChanger6);
        _inventoryChanger7 = new(_playerHandler.currentPlayerInventory._items[6]); AddToObserver(_inventoryChanger7);
        _inventoryChanger8 = new(_playerHandler.currentPlayerInventory._items[7]); AddToObserver(_inventoryChanger8);
        _inventoryChanger9 = new(_playerHandler.currentPlayerInventory._items[8]); AddToObserver(_inventoryChanger9);
        _inventoryChanger10 = new(_playerHandler.currentPlayerInventory._items[9]); AddToObserver(_inventoryChanger10);
    }

    private void ClearUIObserver()
    {
        notifyScript.RemoveObserver(_textChanger);
    }

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
    private void AddToObserver(InventoryChanger invchanger)
    {
        notifyScript.AddInventoryObserver(invchanger);
    }

    private void RemoveFromObserver(InventoryChanger invchanger)
    {
        notifyScript.RemoveInventoryObserver(invchanger);
    }
}