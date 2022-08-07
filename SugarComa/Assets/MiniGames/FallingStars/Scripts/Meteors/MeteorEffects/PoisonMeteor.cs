using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class PoisonMeteor : MonoBehaviour
    {
        #region Properties
        [SerializeField] int _duration = 3;
        [SerializeField] int _poisonDuration = 5;
        [SerializeField] float _damage;
        public bool isPlayerIn;
        private Coroutine insideCoroutine, outsideCoroutine;
        private int _localDuration;
        #endregion

        #region OtherComponents
        [SerializeField] Meteor _meteor;
        #endregion

        private void Awake()
        {
            _meteor.OnMeteorDisable += ResetMeteor;
        }
        private void OnEnable()
        {
            StartCoroutine(CountdownTimer());
        }
        private void DamagePlayer(PlayerSpecs player, float damage)
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
                DamagePlayer(playerSpecs, damage);
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator CountdownTimer()
        {
            _localDuration = _duration;
            while (_duration > 0)
            {
                _duration--;
                yield return new WaitForSeconds(1f);
            }
            MiniGameController.Instance.AddToPool(_meteor);
        }

        private void ResetMeteor()
        {
            print("poisonmeteor resetted");
            _duration = _localDuration;
        }
    }
}