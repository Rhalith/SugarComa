using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class ExplosionMeteor : MonoBehaviour
    {
        #region Properties
        [SerializeField] float _maxExplosionRatio;
        [SerializeField] float _explosionForce = 30; // 1 is equal to 0.024 seconds in air
        [SerializeField] float _explosionDistributionRatio = 10;
        #endregion

        #region OtherComponents
        [SerializeField] private GameObject _meteorShadow;
        public Vector3[] _localPositions = new Vector3[3];
        public List<GameObject> _miniEffects = new List<GameObject>(3);
        public List<GameObject> _miniShadows = new List<GameObject>(3);
        public List<GameObject> _miniObjects = new List<GameObject>(3);
        #endregion

        private void OnEnable()
        {
            DistributeMeteors();
        }

        private void DistributeMeteors()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
            int i = 0;
            foreach (Collider nearby in colliders)
            {
                Rigidbody rig = nearby.GetComponent<Rigidbody>();
                if (rig != null && rig.gameObject.CompareTag("MiniMeteor"))
                {
                    _localPositions[i] = nearby.gameObject.transform.position;
                    _miniObjects.Add(nearby.gameObject);
                    _explosionDistributionRatio = Random.Range(10, _maxExplosionRatio);
                    // rig.AddExplosionForce(_explosionRatio, transform.position, 5f);
                    rig.AddForce(rig.gameObject.transform.localPosition * _explosionForce, ForceMode.Force);
                    rig.velocity +=
                        new Vector3(rig.gameObject.transform.localPosition.x, 0,
                            rig.gameObject.transform.localPosition.z) * _explosionDistributionRatio;
                    Vector3 distance = rig.velocity * 1.05f;
                    distance += rig.transform.position;
                    GameObject shadow = Instantiate(_meteorShadow, new Vector3(distance.x, 0, distance.z), Quaternion.identity,
                        transform);
                    _miniEffects[i].transform.position = new Vector3(distance.x, 0, distance.z);
                    _miniShadows.Add(shadow);
                    i++;
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