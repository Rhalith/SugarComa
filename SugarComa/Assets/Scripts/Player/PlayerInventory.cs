using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> _itemDictionary;

    public GameObject inventoryUI;
    public List<InventoryItem> inventory { get; private set; }

    private void Awake()
    {
        inventory = new List<InventoryItem>();
        _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public InventoryItem GetItem(InventoryItemData refData)
    {
        if(_itemDictionary.TryGetValue(refData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void AddItem(InventoryItemData refData)
    {
        if (_itemDictionary.TryGetValue(refData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(refData);
            inventory.Add(newItem);
            _itemDictionary.Add(refData, newItem);
        }
    }

    public void RemoveItem(InventoryItemData refData)
    {
        if (_itemDictionary.TryGetValue(refData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                _itemDictionary.Remove(refData);
            }
        }
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
    }
    public void CloseInventory()
    {
        inventoryUI.SetActive(false);
    }
}



[Serializable]
public class InventoryItem
{
    public InventoryItemData data { get; private set; }

    public int stackSize { get; private set; }

    public InventoryItem(InventoryItemData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }
    public void RemoveFromStack()
    {
        stackSize--;
    }
}