using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts
{
    public interface IMeteor
    {
        abstract void CheckType(MeteorType type);
        abstract void CheckHit(Collision collision = null, Collider collider = null);
    }
    public class Meteor : MonoBehaviour, IMeteor
    {
        [SerializeField] MeteorType type;
        [SerializeField] Mesh _currentMesh;
        public void CheckHit(Collision collision = null, Collider collider = null)
        {
            if(collision != null)
            {
                if (collision.transform.gameObject.CompareTag("Plane"))
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (collider.transform.gameObject.CompareTag("MeteorShadow"))
                {
                    collider.transform.gameObject.SetActive(false);
                }
            }
        }
        //TODO
        public void CheckType(MeteorType type)
        {
            switch (type)
            {
                case MeteorType.classic:
                    _currentMesh = MiniGameManager.MeteorMeshes.classic;
                    break;
                case MeteorType.explosion:
                    _currentMesh = MiniGameManager.MeteorMeshes.explosion;
                    break;
                case MeteorType.poison:
                    _currentMesh = MiniGameManager.MeteorMeshes.poison;
                    break;
                case MeteorType.sticky:
                    _currentMesh = MiniGameManager.MeteorMeshes.sticky;
                    break;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            CheckHit(collision, null);
        }

        void OnTriggerEnter(Collider collider)
        {
            CheckHit(null, collider);
        }
    }

    public enum MeteorType
    {
        classic,
        explosion,
        poison,
        sticky
    }
}