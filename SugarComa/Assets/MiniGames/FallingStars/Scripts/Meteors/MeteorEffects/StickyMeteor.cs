using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorObjects;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class StickyMeteor : MonoBehaviour
    {
        #region Properties
        [SerializeField] private int _duration = 6;
        [SerializeField] private int _effectDuration = 2;
        [SerializeField] private float _slowEffectRatio;
        private readonly List<PlayerManager> _players = new();
        #endregion

        #region OtherComponents
        [SerializeField] private Meteor _meteor;
        #endregion
        private void Awake()
        {
            _meteor.OnMeteorDisable += ResetMeteor;
        }
        private void OnEnable()
        {
            StartCoroutine(TimerCountdown());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
                _players.Add(playerManager);
                playerManager.StartNumerator(_meteor.Type, 0, _effectDuration, _slowEffectRatio);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
                _players.Remove(playerManager);
                playerManager.StopNumerator(_meteor.Type);
            }
        }

        private IEnumerator TimerCountdown()
        {
            yield return new WaitForSeconds(_duration);
            MiniGameController.Instance.AddToPool(_meteor);
        }
        private void ResetMeteor()
        {
            foreach (var player in _players)
            {
                print("boþ ama girdi");
                player.StopNumerator(_meteor.Type);
            }
            _players.Clear();
            StopAllCoroutines();

        }
    }
}