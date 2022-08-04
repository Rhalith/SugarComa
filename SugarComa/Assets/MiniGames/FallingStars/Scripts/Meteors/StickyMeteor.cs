using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class StickyMeteor : MonoBehaviour, IMeteorHit
    {
        #region Properties
        [SerializeField] int _duration;
        [SerializeField] int _effectDuration;
        [SerializeField] float _slowEffectRatio;
        public bool isPlayerIn;
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
        }
        public void DamagePlayer(PlayerSpecs player, float damage)
        {
            
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
    }
}