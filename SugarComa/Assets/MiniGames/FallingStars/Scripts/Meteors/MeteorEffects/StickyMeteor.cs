using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
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
                StartCoroutine(StickyEffect(playerSpec, _effectDuration, _slowEffectRatio));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerSpecs playerSpec = other.GetComponent<PlayerSpecs>();
                ResetPlayerSpeed(playerSpec, playerSpec._localMoveSpeed, playerSpec._localRotationSpeed);
            }
        }
        private void SlowDownPlayer(PlayerSpecs player, float ratio, float moveSpeed, float rotationSpeed)
        {
            player._moveSpeed = moveSpeed / ratio;
            player._rotationSpeed = rotationSpeed;
        }

        private void StopPlayer(PlayerSpecs player)
        {
            player._moveSpeed = 0;
            player._rotationSpeed = 0;
        }

        private void ResetMeteor()
        {
            print("stickmeteor resetted");
            _duration = _localDuration;
        }
        private void ResetPlayerSpeed(PlayerSpecs player, float moveSpeed, float rotationSpeed)
        {
            player._moveSpeed = moveSpeed;
            player._rotationSpeed = rotationSpeed;
        }
        IEnumerator StickyEffect(PlayerSpecs player, int duration, float ratio)
        {
            player._localMoveSpeed = player._moveSpeed;
            player._localRotationSpeed = player._rotationSpeed;

            StopPlayer(player);
            int localDuration = 0;
            while (localDuration < duration)
            {
                yield return new WaitForSeconds(1f);
                localDuration++;
            }
            SlowDownPlayer(player, ratio, player._localMoveSpeed, player._localRotationSpeed);
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
    }
}