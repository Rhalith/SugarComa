using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryChanger : IObserver
{
    public ItemObject _item;

    public InventoryChanger(ItemObject item)
    {
        this._item = item;
    }

    public void OnNotify()
    {
        _item.OnAddItem();
    }
}
