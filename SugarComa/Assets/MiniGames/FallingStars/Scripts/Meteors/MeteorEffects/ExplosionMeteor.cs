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
        [SerializeField] private float _maxExplosionRatio;
        [SerializeField] private float _explosionForce = 30;
        [SerializeField] private float _explosionDistributionRatio = 10;
        #endregion

        #region OtherComponents
        public List<Vector3> LocalPositions { get => _localPositions; private set => _localPositions = value; }
        public List<GameObject> MiniEffects { get => _miniEffects; private set => _miniEffects = value; }
        public List<GameObject> MiniShadows { get => _miniShadows; private set => _miniShadows = value; }
        public List<GameObject> MiniObjects { get => _miniObjects; private set => _miniObjects = value; }

        #region SerializeFields
        [SerializeField] private GameObject _meteorShadow;
        [SerializeField] private Meteor _meteor;
        [SerializeField] private List<Vector3> _localPositions = new();
        [SerializeField] private List<GameObject> _miniEffects = new();
        [SerializeField] private List<GameObject> _miniShadows = new();
        [SerializeField] private List<GameObject> _miniObjects = new();


        #endregion
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
                    LocalPositions.Add(nearby.gameObject.transform.localPosition);
                    MiniObjects.Add(nearby.gameObject);
                    ThrowMiniMeteor(rig, i);
                    i++;
                }
            }
        }

        private void ResetMeteor()
        {
            print("explosionmeteor resetted");
            for (int i = 0; i < MiniObjects.Count; i++)
            {
                MiniObjects[i].transform.localPosition = LocalPositions[i];
                MiniObjects[i].SetActive(true);
            }
            MiniObjects.Clear();
            MiniShadows.Clear();
            LocalPositions.Clear();
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
            MiniEffects[i].transform.position = new Vector3(distance.x, 0, distance.z);
            MiniShadows.Add(shadow);
        }
    }
}