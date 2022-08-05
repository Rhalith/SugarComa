using System.Collections;
using System.Collections.Generic;
using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorObjects
{
    public class MeteorObject : MonoBehaviour
    {
        #region Events
        public delegate void MeteorAction();

        public MeteorAction OnMeteorHit;
        #endregion

        private void Awake()
        {
            OnMeteorHit += DisableObject;
        }
        void OnTriggerEnter(Collider collider)
        {
            CheckHit(collider);
        }


        /// <summary>
        /// Invokes at the end of MeteorAnimation
        /// </summary>
        public void OnHit()
        {
            OnMeteorHit?.Invoke();
        }
        private void CheckHit(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerSpecs>().KillPlayer();
            }
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }

    }
}