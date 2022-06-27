using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] List<GameObject> _items;

    [SerializeField] public GameObject _playerInventory;

    public static bool _isItemUsing;

    public GameObject _current;

    public void UseItem(int index)
    {
        _current = _items[index];
        _items[index].SetActive(true);
        _playerInventory.SetActive(false);
        _isItemUsing = true;
    }

    public void CloseItem()
    {
        _isItemUsing = false;
        _current.SetActive(false);
    }

    public void UseCurrentItem()
    {
        if (_current.GetComponent<BoxGloves>() != null)
        {
            _current.GetComponent<BoxGloves>().UseItem();
        }
    }
}
