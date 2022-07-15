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

    [SerializeField] PlayerMovement _playerMovement;

    public Button _button; 
    public void OnAddItem()
    {
        playerInventory.AddItem(referenceItem);
    }
    public void RemoveItem()
    {
        playerInventory.RemoveItem(referenceItem);
    }
    
    public void NotifyInventory()
    {
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
        if(item != null)
        {
            text.text = item.stackSize.ToString();
        }
        else
        {
            text.text = 0.ToString();
        }
        gameObject.GetComponent<Image>().sprite = referenceItem.icon;
    }
}
