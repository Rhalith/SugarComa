using Assets.MiniGames.HoleInTheWall.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Obstacles
{
    public class PlaneDeath : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerDeath>().KillPlayer();
            }
        }
    }
}