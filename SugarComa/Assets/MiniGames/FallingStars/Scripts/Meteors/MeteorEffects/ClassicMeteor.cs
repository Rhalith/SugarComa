using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using Assets.MiniGames.FallingStars.Scripts.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class ClassicMeteor : MonoBehaviour
    {
        #region Properties
        private Vector3 _localScale;
        private readonly List<PlayerManager> _players = new();

        #region SeralizeFields
        [SerializeField] private int _duration = 4;
        [SerializeField] private float _damage;
        [SerializeField] private float _upScaleValue;
        [SerializeField] private float _addValue = 30f;
        #endregion
        #endregion
        #region OtherComponents
        [SerializeField] private Meteor _meteor;
        private Material _effectMaterial;
        #endregion

        private void Awake()
        {
            _localScale = transform.localScale;
            _meteor.OnMeteorDisable += ResetMeteor;
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(UpScaleMeteor), 0.2f, 0.1f);
            StartCoroutine(TimerCountdown());
            _effectMaterial = GetComponent<Renderer>().material;
            //StartCoroutine(ChangeMaterial(_effectMaterial, "_z", 0.01f));
            ChangeMaterial(_effectMaterial, "_z", _duration+2f, _addValue);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
                _players.Add(playerManager);
                playerManager.StartNumerator(_meteor.Type, _damage);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
                _players.Remove(playerManager);
                playerManager.StopNumerator(_meteor.Type, _damage);
            }
        }

        private void UpScaleMeteor()
        {
            transform.localScale = new Vector3(transform.localScale.x + _upScaleValue / 10, transform.localScale.y + _upScaleValue / 10, transform.localScale.z + _upScaleValue / 10);
        }

        private IEnumerator TimerCountdown()
        {
            yield return new WaitForSeconds(_duration);
            MiniGameController.Instance.AddToPool(_meteor);
        }

        //TODO DOTWeen ile bir dene.
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
        private void ResetMeteor()
        {
            _effectMaterial.SetFloat("_z", 0);
            transform.localScale = _localScale;
            foreach (var player in _players)
            {
                player.StopNumerator(_meteor.Type);
            }
            _players.Clear();
            StopAllCoroutines();
            CancelInvoke();
            gameObject.SetActive(false);
            MiniGameController.Instance.AddToPool(_meteor);
        }
    }
}