using System;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using Random = UnityEngine.Random;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class ExplosionMeteor : MonoBehaviour
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

        [SerializeField] private GameObject _miniMeteor;
        #endregion

        private void OnEnable()
        {
            DistributeMeteors();
            _localDuration = _duration;
        }

        private void DistributeMeteors()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

          foreach (Collider nearby in colliders)
            {
                print(nearby);
                Rigidbody rig = nearby.GetComponent<Rigidbody>();
                if (rig != null && rig.gameObject.CompareTag("MiniMeteor"))
                {
                    _explosionDistributionRatio = Random.Range(10, _maxExplosionRatio);
                    // rig.AddExplosionForce(_explosionRatio, transform.position, 5f);
                    rig.AddForce(rig.gameObject.transform.localPosition * _explosionForce, ForceMode.Force);
                    rig.velocity +=
                        new Vector3(rig.gameObject.transform.localPosition.x, 0,
                            rig.gameObject.transform.localPosition.z) * _explosionDistributionRatio;
                    Vector3 distance = rig.velocity * 1.05f;
                    distance += rig.transform.position;
                    Instantiate(_currentShadow, new Vector3(distance.x, 0, distance.z), Quaternion.identity,
                        transform);
                }
            }
          /*
          for (int i = 0; i < 3; i++)
          {
              Rigidbody rig;
              
              GameObject instance = Instantiate(_miniMeteor, getMiniMeteorPos()+transform.position, Quaternion.identity,
                  transform);
              rig = instance.GetComponent<Rigidbody>();
              _explosionDistributionRatio = Random.Range(10, _maxExplosionRatio);
              // rig.AddExplosionForce(_explosionRatio, transform.position, 5f);
              rig.AddForce(rig.gameObject.transform.localPosition * _explosionForce, ForceMode.Force);
              rig.velocity +=
                  new Vector3(rig.gameObject.transform.localPosition.x, 0,
                      rig.gameObject.transform.localPosition.z) *_explosionDistributionRatio;
              Vector3 distance = rig.velocity;
              distance += rig.transform.position;
              //print("distance:" + distance + "name:" + rig.name);
              Instantiate(_currentShadow, new Vector3(distance.x, 0, distance.z), Quaternion.identity,
                  transform);
            }*/
        }

        private Vector3 getMiniMeteorPos()
        {
            float x = Random.Range(0f, 1f);
            float z = MathF.Sqrt(1f - Mathf.Pow(x, 2));
            Vector3 vector = new Vector3(x,1f,z);
            
            print(vector);
            return vector;
        }
    }
}