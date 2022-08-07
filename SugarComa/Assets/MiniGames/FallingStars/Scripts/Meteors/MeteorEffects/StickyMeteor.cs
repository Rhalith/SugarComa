using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class StickyMeteor : MonoBehaviour
    {
        #region Properties
        [SerializeField] int _duration = 6;
        [SerializeField] int _effectDuration = 2;
        [SerializeField] float _slowEffectRatio;
        private int _localDuration;
        private List<PlayerSpecs> _playerSpecs = new();
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
                PlayerSpecs playerSpec = other.GetComponent<PlayerSpecs>();
                _playerSpecs.Add(playerSpec);
                StartCoroutine(StickyEffect(playerSpec, _effectDuration, _slowEffectRatio));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerSpecs playerSpec = other.GetComponent<PlayerSpecs>();
                _playerSpecs.Remove(playerSpec);
                playerSpec.ResetPlayerSpeed();
            }
        }
        IEnumerator StickyEffect(PlayerSpecs player, int duration, float ratio)
        {
            player.StopPlayerMovement();
            int localDuration = 0;
            while (localDuration < duration)
            {
                yield return new WaitForSeconds(1f);
                localDuration++;
            }
            player.SlowDownPlayer(ratio);
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
            print("stickmeteor resetted");
            if (_playerSpecs.Count > 0)
            {
                foreach (var player in _playerSpecs)
                {
                    player.ResetPlayerSpeed();
                }

            }
            _playerSpecs.Clear();
            _duration = _localDuration;
            StopAllCoroutines();

        }
    }
}