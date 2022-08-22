using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class PoisonMeteor : MonoBehaviour
    {
        #region Properties
        private readonly List<PlayerManager> _players = new();
        #region SeralizeFields
        [SerializeField] private int _duration = 3;
        [SerializeField] private int _poisonDuration = 5;
        [SerializeField] private float _damage;
        #endregion
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
                playerManager.StopNumerator(_meteor.Type, _damage, _poisonDuration);
            }
        }

        private IEnumerator CountdownTimer()
        {
            yield return new WaitForSeconds(_duration);
            MiniGameController.Instance.AddToPool(_meteor);
        }

        private void ResetMeteor()
        {
            foreach (var player in _players)
            {
                player.StopNumerator(_meteor.Type);
            }
            StopAllCoroutines();
        }
    }
}