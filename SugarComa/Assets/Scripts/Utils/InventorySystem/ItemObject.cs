using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    [SerializeField] TMP_Text text;

    [SerializeField] PlayerInventory playerInventory;
    public void OnAddItem()
    {
        playerInventory.AddItem(referenceItem);
        ChangeInventory(playerInventory.GetItem(referenceItem));
    }

    public void ChangeInventory(InventoryItem item)
    {
        text.text = item.stackSize.ToString();
    }
}
