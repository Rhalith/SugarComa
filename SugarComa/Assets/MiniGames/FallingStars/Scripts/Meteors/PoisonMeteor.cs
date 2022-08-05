using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class PoisonMeteor : MonoBehaviour
    {
        #region Properties
        [SerializeField] int _duration;
        [SerializeField] float _damage;
        public bool isPlayerIn;
        private Coroutine insideCoroutine, outsideCoroutine;
        #endregion
        public void DamagePlayer(PlayerSpecs player, float damage)
        {
            player._health -= damage;
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
                outsideCoroutine = StartCoroutine(other.GetComponent<PlayerSpecs>().PoisonEffect(_duration, _damage));
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