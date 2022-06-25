using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    [SerializeField] TMP_Text text;

    [SerializeField] PlayerInventory playerInventory;

    public Button _button; 
    public void OnAddItem()
    {
        playerInventory.AddItem(referenceItem);
        ChangeInventory(playerInventory.GetItem(referenceItem));
    }
    public void RemoveItem()
    {
        playerInventory.RemoveItem(referenceItem);
        ChangeInventory(playerInventory.GetItem(referenceItem));
    }

    public int CheckItemCount()
    {
        if(playerInventory.GetItem(referenceItem) == null)
        {
            return 0;
        }
        return playerInventory.GetItem(referenceItem).stackSize;
    }

    public void ChangeInventory(InventoryItem item)
    {
        text.text = item.stackSize.ToString();
        gameObject.GetComponent<Image>().sprite = item.Data.icon;
    }
}
