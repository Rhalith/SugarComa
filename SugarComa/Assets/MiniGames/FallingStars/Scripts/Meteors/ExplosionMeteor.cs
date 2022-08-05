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
        [SerializeField] float _maxExplosionRatio;
        [SerializeField] float _explosionForce = 30; // 1 is equal to 0.024 seconds in air
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
            DistributeMeteors();
            _localDuration = _duration;
        }

        public void DamagePlayer(PlayerSpecs player, float damage)
        {
            throw new System.NotImplementedException();
        }

        public void KillPlayer(PlayerSpecs player)
        {
            player._health = 0;
            player._isDead = true;
        }

        private void DistributeMeteors()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

            //transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            foreach (Collider nearby in colliders)
            {
                print(nearby);
                Rigidbody rig = nearby.GetComponent<Rigidbody>();
                if (rig != null && rig.gameObject.tag.Equals("MiniMeteor"))
                {
                    _explosionDistributionRatio = Random.Range(10, _maxExplosionRatio);
                    // rig.AddExplosionForce(_explosionRatio, transform.position, 5f);
                    rig.AddForce(rig.gameObject.transform.localPosition * _explosionForce, ForceMode.Force);
                    rig.velocity +=
                        new Vector3(rig.gameObject.transform.localPosition.x, 0,
                            rig.gameObject.transform.localPosition.z) * _explosionDistributionRatio;
                    Vector3 distance = rig.velocity * 1.05f;
                    distance += rig.transform.position;
                    //print("distance:" + distance + "name:" + rig.name);
                    Instantiate(_currentShadow, new Vector3(distance.x,0,distance.z), Quaternion.identity);
                }
            }
        }
    }
}