using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class ExplosionMeteor : MonoBehaviour, IMeteorHit
    {
        [SerializeField] Meteor _currentMeteor;
        public void DamagePlayer(PlayerSpecs player)
        {
            throw new System.NotImplementedException();
        }

        public bool IsPlayerIn()
        {
            throw new System.NotImplementedException();
        }

        public void KillPlayer(PlayerSpecs player)
        {
            throw new System.NotImplementedException();
        }

    }
}