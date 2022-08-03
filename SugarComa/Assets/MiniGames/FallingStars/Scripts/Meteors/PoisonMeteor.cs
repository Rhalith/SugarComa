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
        private Coroutine insideCoroutine, outsideCoroutine;
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
            _localDuration = _duration;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerIn = true;
                if(outsideCoroutine != null) StopCoroutine(outsideCoroutine);
                insideCoroutine = StartCoroutine(Poison(_damage, other.GetComponent<PlayerSpecs>()));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerIn = false;
                if (insideCoroutine != null) StopCoroutine(insideCoroutine);
                outsideCoroutine = StartCoroutine(other.GetComponent<PlayerSpecs>().PoisonEffect(_localDuration, _damage));
            }
        }

        private IEnumerator Poison(float damage, PlayerSpecs playerSpecs)
        {
            while (isPlayerIn)
            {
                playerSpecs._health -= damage;
                yield return new WaitForSeconds(1f);
            }
        }

    }
}