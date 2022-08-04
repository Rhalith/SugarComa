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
        [SerializeField] float _explosionForce = 30;
        [SerializeField] float _explosionDistributionRatio = 10;
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
        }

        public void DamagePlayer(PlayerSpecs player, float damage)
        {
            throw new System.NotImplementedException();
        }

        public void KillPlayer(PlayerSpecs player)
        {
            throw new System.NotImplementedException();
        }

        private void DistributeMeteors()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

            foreach (Collider nearby in colliders)
            {
                print(nearby);
                Rigidbody rig = nearby.GetComponent<Rigidbody>();
                if (rig != null && rig.gameObject.tag.Equals("MiniMeteor"))
                {
                    // rig.AddExplosionForce(_explosionRatio, transform.position, 5f);
                    rig.AddForce(rig.gameObject.transform.localPosition * _explosionForce);
                    rig.velocity +=
                        new Vector3(rig.gameObject.transform.localPosition.x, 0,
                            rig.gameObject.transform.localPosition.z) * _explosionDistributionRatio;
                }
            }
        }
    }
}