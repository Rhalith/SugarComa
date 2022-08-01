using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.MainBoard.Scripts.Utils.InventorySystem;

namespace Assets.MainBoard.Scripts.Player.Items
{
    public class PlayerInventory : MonoBehaviour
    {
        private Dictionary<InventoryItemData, InventoryItem> _itemDictionary;

        public GameObject inventoryUI;
        public List<InventoryItem> Inventory { get; private set; }

        public List<ItemObject> _items;

        private void Awake()
        {
            Inventory = new List<InventoryItem>();
            _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        }

        public InventoryItem GetItem(InventoryItemData refData)
        {
            if (_itemDictionary.TryGetValue(refData, out InventoryItem value))
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
                InventoryItem newItem = new(refData);
                Inventory.Add(newItem);
                _itemDictionary.Add(refData, newItem);
            }
        }

        public void RemoveItem(InventoryItemData refData)
        {
            if (_itemDictionary.TryGetValue(refData, out InventoryItem value))
            {
                value.RemoveFromStack();
                if (value.stackSize == 0)
                {
                    Inventory.Remove(value);
                    _itemDictionary.Remove(refData);
                }
            }
        }

        public void OpenInventory()
        {
            inventoryUI.SetActive(true);
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].CheckItemCount() <= 0)
                {
                    _items[i]._button.interactable = false;
                }
                else
                {
                    _items[i]._button.interactable = true;
                }
            }
        }
        public void CloseInventory()
        {
            inventoryUI.SetActive(false);
        }
    }



    [Serializable]
    public class InventoryItem
    {
        public InventoryItemData Data { get; private set; }

        public int stackSize { get; private set; }

        public InventoryItem(InventoryItemData source)
        {
            Data = source;
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
}