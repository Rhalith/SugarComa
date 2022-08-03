using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class ClassicMeteor : MonoBehaviour, IMeteorHit
    {
        #region Properties
        [SerializeField] int _duration;
        [SerializeField] float _damage;
        [SerializeField] float _upScaleValue;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(DamageToPlayer(other.gameObject.GetComponent<PlayerSpecs>(), _damage));
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

        private void UpScaleMeteorEffect()
        {
            transform.localScale = new Vector3(transform.localScale.x + _upScaleValue / 100, transform.localScale.y, transform.localScale.z + _upScaleValue / 100);
        }
        private void WhileDuration()
        {
            if (_duration > 0) _duration--;
            else 
            {
                CancelInvoke();
                gameObject.SetActive(false);
                _duration = _localDuration;
                transform.localScale = _localScale;
            }
        }
        IEnumerator DamageToPlayer(PlayerSpecs player = null, float damage = 0)
        {
            while (true)
            {
                DamagePlayer(player, damage);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}