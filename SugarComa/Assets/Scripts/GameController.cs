using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameController : MonoBehaviour
{
    public PlayerObjects playerObjects;

    public Inventory inventory;

    readonly NotifyScript notifyScript = new();

    private void Start()
    {
        TextChanger textChanger = new(playerObjects.playerGold, playerObjects.playerHealth,playerObjects.playerGoblet, playerObjects.playerCollector);
        notifyScript.AddObserver(textChanger);

        InstanceInventoryObjects();
    }

    public void ChangeText()
    {
        notifyScript.Notify();
    }

    public void ChangeInventory()
    {
        notifyScript.NotifyInventory();
    }

    private void InstanceInventoryObjects()
    {
        InventoryChanger inventoryChanger = new(inventory._item); AddToObserver(inventoryChanger);
        InventoryChanger inventoryChanger1 = new(inventory._item1); AddToObserver(inventoryChanger1);
        InventoryChanger inventoryChanger2 = new(inventory._item2); AddToObserver(inventoryChanger2);
        InventoryChanger inventoryChanger3 = new(inventory._item3); AddToObserver(inventoryChanger3);
        InventoryChanger inventoryChanger4 = new(inventory._item4); AddToObserver(inventoryChanger4);
        InventoryChanger inventoryChanger5 = new(inventory._item5); AddToObserver(inventoryChanger5);
        InventoryChanger inventoryChanger6 = new(inventory._item6); AddToObserver(inventoryChanger6);
        InventoryChanger inventoryChanger7 = new(inventory._item7); AddToObserver(inventoryChanger7);
        InventoryChanger inventoryChanger8 = new(inventory._item8); AddToObserver(inventoryChanger8);
        InventoryChanger inventoryChanger9 = new(inventory._item9); AddToObserver(inventoryChanger9);
    }

    private void AddToObserver(InventoryChanger invchanger)
    {
        notifyScript.AddInventoryObserver(invchanger);
    }
}

[System.Serializable]
public class PlayerObjects
{
    public PlayerCollector playerCollector;
    public TMP_Text playerGold, playerHealth, playerGoblet;
}
[System.Serializable]
public class Inventory
{
    public ItemObject _item;
    public ItemObject _item1;
    public ItemObject _item2;
    public ItemObject _item3;
    public ItemObject _item4;
    public ItemObject _item5;
    public ItemObject _item6;
    public ItemObject _item7;
    public ItemObject _item8;
    public ItemObject _item9;
}
