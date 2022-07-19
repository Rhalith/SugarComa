using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] List<GameObject> _items;
    [SerializeField] Animator _playerAnimator;
    [SerializeField] public GameObject _playerInventory;

    public static bool _isItemUsing;

    public GameObject _current;

    public void UseItem(int index)
    {
        _current = _items[index];
        _items[index].SetActive(true);
        _playerInventory.SetActive(false);
        _isItemUsing = true;
        _playerAnimator.SetBool("itemUsing", true);
    }

    public void CloseItem()
    {
        _isItemUsing = false;
        CloseAllItems();
    }

    public void UseCurrentItem()
    {
        if (_current.GetComponent<BoxGloves>() != null)
        {
            _current.GetComponent<BoxGloves>().UseItem();
        }
    }

    private void CloseAllItems()
    {
        _playerAnimator.SetBool("itemUsing", false);
        if (ItemUsing.BoxGlovesUsing) _playerAnimator.SetBool("boks", false);
    }
}

public class ItemUsing
{
    public static bool BoxGlovesUsing;
}