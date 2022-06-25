using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGloves : MonoBehaviour, IDamageItems
{

    [SerializeField] ItemObject _itemObject;
    [SerializeField] GameObject _player;
    [SerializeField] int damage;
    public void DamageHealth(PlayerCollector playerCollector)
    {
        playerCollector.health -= damage;
    }

    public void UseItem()
    {
        _itemObject.RemoveItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && other != _player)
        {
            DamageHealth(other.gameObject.GetComponent<PlayerCollector>());
        }
    }
}
