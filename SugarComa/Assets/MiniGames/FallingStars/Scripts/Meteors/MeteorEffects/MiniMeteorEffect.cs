using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects
{
    public class MiniMeteorEffect : MonoBehaviour
    {
        #region Properties
        private Vector3 _localScale;
        private readonly List<PlayerManager> _players = new();
        #region SerializeFields
        [SerializeField] private int _duration = 4;
        [SerializeField] private float _damage;
        [SerializeField] private float _upScaleValue;
        #endregion
        #endregion

        #region OtherComponents
        [SerializeField] private Meteor _meteor;
        [SerializeField] private GameObject _explosionMeteor;
        private Material _effectMaterial;
        #endregion
        private void Awake()
        {
            _meteor.OnMeteorDisable += ResetMeteor;
            _localScale = transform.localScale;
        }
        private void OnEnable()
        {
            InvokeRepeating(nameof(UpScaleMeteorEffect), 0.2f, 0.1f);
            _effectMaterial = GetComponent<Renderer>().material;
            StartCoroutine(ChangeMaterial(_effectMaterial, "_z", 0.01f));
            StartCoroutine(CountdownTimer());
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
                playerManager.StopNumerator(_meteor.Type);
            }
        }
        private void UpScaleMeteorEffect()
        {
            transform.localScale = new Vector3(transform.localScale.x + _upScaleValue / 10, transform.localScale.y + _upScaleValue /10, transform.localScale.z + _upScaleValue / 10);
        }
        private IEnumerator CountdownTimer()
        {
            yield return new WaitForSeconds(_duration);
            MiniGameController.Instance.AddToPool(_meteor);
        }
        private IEnumerator ChangeMaterial(Material material, string refID, float waitDuration)
        {
            float _startValue = Random.Range(0, 50);
            while (true)
            {
                material.SetFloat(refID, _startValue);
                _startValue += 0.01f;
                yield return new WaitForSeconds(waitDuration);
            }
        }
        private void ResetMeteor()
        {
            transform.localScale = _localScale;
            foreach (var player in _players)
            {
                print("explosion");
                player.StopNumerator(_meteor.Type);
            }
            _players.Clear();
            StopAllCoroutines();
            CancelInvoke();
            _explosionMeteor.SetActive(false);
            gameObject.SetActive(false);
            MiniGameController.Instance.AddToPool(_meteor);
        }
    }
}