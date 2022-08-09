using System.Collections;
using System.Collections.Generic;
using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorObjects
{
    public class MeteorObject : MonoBehaviour
    {
        [SerializeField] private Meteor _meteor;

        /// <summary>
        /// Invokes at the end of MeteorAnimation
        /// </summary>
        public void OnHit()
        {
            _meteor.OnMeteorHit(true);
        }
        private void OnTriggerEnter(Collider collider)
        {
            CheckHit(collider);
        }

        private void CheckHit(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerManager playerManager = collider.gameObject.GetComponent<PlayerManager>();
                playerManager.KillPlayer();
            }
        }

    }
}