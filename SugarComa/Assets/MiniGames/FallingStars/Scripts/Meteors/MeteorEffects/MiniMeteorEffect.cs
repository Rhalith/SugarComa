using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class MiniMeteorEffect : MonoBehaviour
    {
        #region Properties
        [SerializeField] int _duration = 4;
        [SerializeField] float _damage;
        [SerializeField] float _upScaleValue;
        private int _localDuration;
        private Vector3 _localScale;
        #endregion

        #region OtherComponents
        [SerializeField] Meteor _meteor;
        [SerializeField] GameObject _explosionMeteor;
        #endregion
        private void Awake()
        {
            _meteor.OnMeteorDisable += ResetMeteor;
        }
        private void OnEnable()
        {
            _localScale = transform.localScale;
            _localDuration = _duration;
            InvokeRepeating(nameof(UpScaleMeteorEffect), 0.2f, 0.1f);
            InvokeRepeating(nameof(WhileDuration), 0f, 1f);
        }
        private void DamagePlayer(PlayerSpecs player, float damage)
        {
            player._health -= damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(DamageToPlayer(other.gameObject.GetComponent<PlayerSpecs>(), _damage));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
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

        private void OnDisable()
        {
            MiniGameController.Instance.AddToPool(_meteor);
        }

        private void ResetMeteor()
        {
            print("minimeteor resetted");
            _duration = _localDuration;
            transform.localScale = _localScale;
            _explosionMeteor.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}