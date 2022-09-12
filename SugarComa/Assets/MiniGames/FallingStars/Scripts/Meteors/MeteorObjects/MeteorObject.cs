using System.Collections;
using System.Collections.Generic;
using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorObjects
{
    public class MeteorObject : MonoBehaviour
    {
        [SerializeField] private Meteor _meteor;
        [SerializeField] private int _animationTime = 2;
        [SerializeField] private float _addValue = 15f;
        private Material _flameMaterial;

        public Material FlameMaterial { get => _flameMaterial; set => _flameMaterial = value; }

        /// <summary>
        /// Invokes at the end of MeteorAnimation
        /// </summary>
        public void OnHit()
        {
            StopAllCoroutines();
            _meteor.OnMeteorHit(true);
        }
        private void OnTriggerEnter(Collider collider)
        {
            CheckHit(collider);
        }

        private void OnEnable()
        {
            ChangeMaterial(FlameMaterial, "_Y", _animationTime+2f, _addValue);
            //StartCoroutine(ChangeMaterial(FlameMaterial, "_Y", 0.01f));
        }
        private void OnDisable()
        {
            FlameMaterial.SetFloat("_Y", 0);
        }

        private void CheckHit(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerManager playerManager = collider.gameObject.GetComponent<PlayerManager>();
                playerManager.KillPlayer();
            }
        }
        //private IEnumerator ChangeMaterial(Material material, string refID, float waitDuration)
        //{
        //    float _startValue = Random.Range(0, 50);
        //    while (true)
        //    {
        //        material.SetFloat(refID, _startValue);
        //        _startValue += 0.1f;
        //        yield return new WaitForSeconds(waitDuration);
        //    }
        //}
        private void ChangeMaterial(Material material, string refID, float waitDuration, float addValue)
        {
            float _startValue = Random.Range(0, 50);
            float _endValue = _startValue + addValue;
            material.SetFloat(refID, _startValue);
            material.DOFloat(_endValue, refID, waitDuration);
        }
    }
}