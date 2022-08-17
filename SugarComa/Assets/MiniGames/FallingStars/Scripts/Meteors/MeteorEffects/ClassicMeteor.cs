using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class ClassicMeteor : MonoBehaviour
    {
        #region Properties
        private Vector3 _localScale;
        private readonly List<PlayerManager> _players = new();

        #region SeralizeFields
        [SerializeField] private int _duration = 4;
        [SerializeField] private float _damage;
        [SerializeField] private float _upScaleValue;
        #endregion
        #endregion
        #region OtherComponents
        [SerializeField] private Meteor _meteor;
        #endregion

        private void Awake()
        {
            _localScale = transform.localScale;
            _meteor.OnMeteorDisable += ResetMeteor;
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(UpScaleMeteor), 0.2f, 0.1f);
            StartCoroutine(TimerCountdown());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
                _players.Add(playerManager);
                playerManager.StartNumerator(_meteor.Type, _damage);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
                _players.Remove(playerManager);
                playerManager.StopNumerator(_meteor.Type, _damage);
            }
        }

        private void UpScaleMeteor()
        {
            transform.localScale = new Vector3(transform.localScale.x + _upScaleValue / 10, transform.localScale.y + _upScaleValue / 10, transform.localScale.z + _upScaleValue / 10);
        }

        private IEnumerator TimerCountdown()
        {
            yield return new WaitForSeconds(_duration);
            MiniGameController.Instance.AddToPool(_meteor);
        }

        private void ResetMeteor()
        {
            transform.localScale = _localScale;
            foreach (var player in _players)
            {
                player.StopNumerator(_meteor.Type);
            }
            _players.Clear();
            StopAllCoroutines();
            CancelInvoke();
            gameObject.SetActive(false);
            MiniGameController.Instance.AddToPool(_meteor);
        }
    }
}