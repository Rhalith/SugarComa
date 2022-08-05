using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class PoisonMeteor : MonoBehaviour
    {
        #region Properties
        [SerializeField] int _duration = 3;
        [SerializeField] int _poisonDuration = 5;
        [SerializeField] float _damage;
        public bool isPlayerIn;
        private Coroutine insideCoroutine, outsideCoroutine;
        #endregion

        private void OnEnable()
        {
            StartCoroutine(CountdownTimer());
        }
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
                outsideCoroutine = StartCoroutine(other.GetComponent<PlayerSpecs>().PoisonEffect(_poisonDuration, _damage));
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

        private IEnumerator CountdownTimer()
        {
            while (_duration > 0)
            {
                _duration--;
                yield return new WaitForSeconds(1f);
            }
            gameObject.SetActive(false);
        }
    }
}