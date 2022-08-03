using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShadow : MonoBehaviour
{
    public bool isPlayerInShadow;
    public List<PlayerSpecs> _playerList;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInShadow = true;
            _playerList.Add(other.GetComponent<PlayerSpecs>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInShadow = false;
            _playerList.Remove(other.GetComponent<PlayerSpecs>());
        }
    }
}
