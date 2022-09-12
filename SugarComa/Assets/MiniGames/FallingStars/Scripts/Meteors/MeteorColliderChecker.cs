using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class MeteorColliderChecker : MonoBehaviour
    {
        private bool _isIn;
        [SerializeField] private Collider _currentChecker;
        [SerializeField] MeshRenderer _shadowRenderer;
        [SerializeField] private Meteor _meteor;
        //public int i;
        public bool IsIn { get => _isIn; set => _isIn = value; }

        //private Transform _left, _right, _up, _bottom;

        //private void Awake()
        //{
        //    _meteor.OnMeteorDisable += ResetChecker;
        //}

        public void ChangeMeteorPosition(Transform left, Transform right, Transform up, Transform bottom)
        {
            //_left = left; _right = right; _up = up; _bottom = bottom;
            //i++;
            float x = Random.Range(left.position.x, right.position.x);
            float z = GetZValue(x, 50);
            _meteor.transform.position = new Vector3(x, 0, z);
            //if (IsIn)
            //{
            //    ChangeMeteorPosition(left, right, up, bottom);
            //}
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ColliderChecker") && other != _currentChecker)
            {
                //ChangeMeteorPosition(_left, _right, _up, _bottom);
                IsIn = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ColliderChecker") && other != _currentChecker)
            {
                IsIn = false;
                _shadowRenderer.enabled = true;
            }
        }
        private float GetZValue(float x, float radius)
        {
            float zValue = Mathf.Pow(Mathf.Pow(radius, 2) - Mathf.Pow(x, 2), 0.5f);
            return Random.Range(-zValue, zValue);
        }

        //private void ResetChecker()
        //{
        //    _shadowRenderer.enabled = false;
        //    _meteor.transform.position = new Vector3(0, 12, 0);
        //    IsIn = false;
        //}
    }
}