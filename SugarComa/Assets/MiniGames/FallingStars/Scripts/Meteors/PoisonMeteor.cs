using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class PoisonMeteor : MonoBehaviour, IMeteorHit
    {
        #region Properties
        [SerializeField] int _duration;
        [SerializeField] float _damage;
        public bool isPlayerIn;
        private int _localDuration;
        private Vector3 _localScale;
        #endregion

        #region OtherComponents
        [SerializeField] Meteor _currentMeteor;
        [SerializeField] MeteorShadow _currentShadow;
        #endregion

        private void OnEnable()
        {
            if (_currentShadow.isPlayerInShadow)
            {
                foreach (PlayerSpecs player in _currentShadow._playerList)
                {
                    KillPlayer(player);
                }
            }
            _localScale = transform.localScale;
            _localDuration = _duration;
            InvokeRepeating("UpScaleMeteorEffect", 0.2f, 0.1f);
            InvokeRepeating("WhileDuration", 0f, 1f);
        }
        public void DamagePlayer(PlayerSpecs player, float damage)
        {
            player._health -= damage;
        }

        public void KillPlayer(PlayerSpecs player)
        {
            player._health = 0;
            player._isDead = true;
        }

        //TODO
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //StartCoroutine();
                isPlayerIn = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
                isPlayerIn = false;
            }
        }

    }
}