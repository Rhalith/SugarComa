using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class ClassicMeteor : MonoBehaviour
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
        #endregion

        private void OnEnable()
        {
            _localScale = transform.localScale;
            _localDuration = _duration;
            InvokeRepeating("UpScaleMeteorEffect", 0.2f, 0.1f);
            InvokeRepeating("WhileDuration", 0f, 1f);
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
                _duration = _localDuration;
                transform.localScale = _localScale;
                MiniGameController.Instance.AddToPool(_meteor);
                gameObject.SetActive(false);
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