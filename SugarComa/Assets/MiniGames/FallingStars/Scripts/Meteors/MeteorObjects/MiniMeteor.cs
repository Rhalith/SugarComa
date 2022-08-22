using Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects;
using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorObjects
{
    public class MiniMeteor : MonoBehaviour
    {
        #region Properties
        [SerializeField] private float _damage;
        #endregion
        #region OtherComponents
        [SerializeField] private ExplosionMeteor _explosionMeteor;
        #endregion

        private void OnEnable()
        {
            EnableListElements(_explosionMeteor.MiniObjects);
            EnableListElements(_explosionMeteor.MiniShadows);
            DisableListElements(_explosionMeteor.MiniEffects);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MeteorShadow"))
            {
                for (int i = 0; i < _explosionMeteor.MiniObjects.Count; i++)
                {
                    _explosionMeteor.MiniObjects[i].transform.position = _explosionMeteor.LocalPositions[i];
                }
                DisableListElements(_explosionMeteor.MiniObjects);
                DisableListElements(_explosionMeteor.MiniShadows);
                EnableListElements(_explosionMeteor.MiniEffects);
            }
            else if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
                playerManager.DamagePlayer(_damage);
            }
        }

        private void EnableListElements(List<GameObject> list)
        {
            foreach (var gameObject in list)
            {
                gameObject.SetActive(true);
            }
        }

        private void DisableListElements(List<GameObject> list)
        {
            foreach (var gameObject in list)
            {
                gameObject.SetActive(false);
            }
        }
    }

}