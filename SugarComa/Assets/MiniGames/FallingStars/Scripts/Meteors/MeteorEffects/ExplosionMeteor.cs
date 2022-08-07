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
        [SerializeField] private Meteor _meteor;
        public List<Vector3> _localPositions = new();
        public List<GameObject> _miniEffects = new();
        public List<GameObject> _miniShadows = new();
        public List<GameObject> _miniObjects = new();
        #endregion


        private void Awake()
        {
            _meteor.OnMeteorDisable += ResetMeteor;
        }
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
                    _localPositions.Add(nearby.gameObject.transform.localPosition);
                    _miniObjects.Add(nearby.gameObject);
                    ThrowMiniMeteor(rig, i);
                    i++;
                }
            }
        }

        private void ResetMeteor()
        {
            print("explosionmeteor resetted");
            for (int i = 0; i < _miniObjects.Count; i++)
            {
                _miniObjects[i].transform.localPosition = _localPositions[i];
                _miniObjects[i].SetActive(true);
            }
            _miniObjects.Clear();
            _miniShadows.Clear();
            _localPositions.Clear();
        }

        private void ThrowMiniMeteor(Rigidbody rig, int i)
        {
            _explosionDistributionRatio = Random.Range(10, _maxExplosionRatio);
            rig.AddForce(rig.gameObject.transform.localPosition * _explosionForce, ForceMode.Force);
            rig.velocity += new Vector3(rig.gameObject.transform.localPosition.x, 0, rig.gameObject.transform.localPosition.z) * _explosionDistributionRatio;
            Vector3 distance = rig.velocity * 1.05f;
            distance += rig.transform.position;
            //TODO shadow oluþturma active et
            GameObject shadow = Instantiate(_meteorShadow, new Vector3(distance.x, 0, distance.z), Quaternion.identity,
                transform);
            _miniEffects[i].transform.position = new Vector3(distance.x, 0, distance.z);
            _miniShadows.Add(shadow);
        }
    }
}