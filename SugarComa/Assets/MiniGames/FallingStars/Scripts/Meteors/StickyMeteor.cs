using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class StickyMeteor : MonoBehaviour, IMeteorHit
    {
        #region Properties
        [SerializeField] int _duration;
        [SerializeField] float _damage;
        #endregion

        #region OtherComponents
        [SerializeField] Meteor _currentMeteor;
        #endregion
        public void DamagePlayer(PlayerSpecs player, float damage)
        {
            
        }

        public void KillPlayer(PlayerSpecs player)
        {
            
        }

    }
}