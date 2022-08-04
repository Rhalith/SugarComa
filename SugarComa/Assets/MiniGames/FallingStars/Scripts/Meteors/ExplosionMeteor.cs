using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class ExplosionMeteor : MonoBehaviour, IMeteorHit
    {
        #region Properties
        [SerializeField] int _duration;
        [SerializeField] float _damage;
        [SerializeField] float _explosionRatio;
        public bool isPlayerIn;
        private int _localDuration;
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
            _localDuration = _duration;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

            foreach (Collider nearby in colliders)
            {
                print(nearby);
                Rigidbody rig = nearby.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(_explosionRatio, transform.position, 5f);
                }
            }
        }

        public void DamagePlayer(PlayerSpecs player, float damage)
        {
            throw new System.NotImplementedException();
        }

        public void KillPlayer(PlayerSpecs player)
        {
            throw new System.NotImplementedException();
        }

    }
}