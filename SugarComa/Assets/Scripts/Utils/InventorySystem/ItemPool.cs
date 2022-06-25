using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] List<GameObject> test;

    [SerializeField] GameObject _playerInventory;

    public static bool _isItemUsing;

    private GameObject _current;

    public void UseItem(int index)
    {
        _current = test[index];
        test[index].SetActive(true);
        _playerInventory.SetActive(false);
        _isItemUsing = true;
    }

    public void CloseItem()
    {
        _isItemUsing = false;
        _current.SetActive(false);
    }
}
