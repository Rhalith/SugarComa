using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class ClassicMeteor : MonoBehaviour, IMeteorHit
    {
        [SerializeField] Meteor _currentMeteor;
        public void DamagePlayer(PlayerSpecs player)
        {
            
        }

        public bool IsPlayerIn()
        {
            return true;
        }

        public void KillPlayer(PlayerSpecs player)
        {
            
        }
    }
}