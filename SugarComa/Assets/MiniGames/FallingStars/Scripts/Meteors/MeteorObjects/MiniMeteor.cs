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
        [SerializeField] float _damage;
        #endregion
        #region OtherComponents
        [SerializeField] ExplosionMeteor _explosionMeteor;
        #endregion

        private void OnEnable()
        {
            EnableListElements(_explosionMeteor._miniObjects);
            EnableListElements(_explosionMeteor._miniShadows);
            DisableListElements(_explosionMeteor._miniEffects);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Plane"))
            {
                for (int i = 0; i < _explosionMeteor._miniObjects.Count; i++)
                {
                    _explosionMeteor._miniObjects[i].transform.position = _explosionMeteor._localPositions[i];
                }
                DisableListElements(_explosionMeteor._miniObjects);
                DisableListElements(_explosionMeteor._miniShadows);
                EnableListElements(_explosionMeteor._miniEffects);
            }
            else if (other.CompareTag("Player"))
            {
                PlayerSpecs playerSpecs = other.GetComponent<PlayerSpecs>();
                DamagePlayer(playerSpecs, _damage);
            }
        }

        private void DamagePlayer(PlayerSpecs player, float damage)
        {
            player._health -= damage;
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