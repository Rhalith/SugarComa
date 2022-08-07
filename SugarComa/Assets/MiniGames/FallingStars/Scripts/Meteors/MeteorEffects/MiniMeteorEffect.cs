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
            _localScale = transform.localScale;
            _localDuration = _duration;
        }
        private void OnEnable()
        {
            InvokeRepeating(nameof(UpScaleMeteorEffect), 0.2f, 0.1f);
            InvokeRepeating(nameof(WhileDuration), 0f, 1f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerSpecs playerSpec = other.gameObject.GetComponent<PlayerSpecs>();
                StartCoroutine(DamageToPlayer(playerSpec, _damage));
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
                MiniGameController.Instance.AddToPool(_meteor);
            }
        }
        IEnumerator DamageToPlayer(PlayerSpecs player = null, float damage = 0)
        {
            while (true)
            {
                player.DamagePlayer(damage);
                yield return new WaitForSeconds(1f);
            }
        }

        private void ResetMeteor()
        {
            print("minimeteor resetted");
            _duration = _localDuration;
            transform.localScale = _localScale;
            StopAllCoroutines();
            CancelInvoke();
            _explosionMeteor.SetActive(false);
            gameObject.SetActive(false);
            MiniGameController.Instance.AddToPool(_meteor);
        }
    }
}