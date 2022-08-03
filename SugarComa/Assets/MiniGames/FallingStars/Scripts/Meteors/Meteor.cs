using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class Meteor : MonoBehaviour
    {

        MeteorType _type;
        [SerializeField] MeshFilter _meteorMesh, _effectMesh;
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] GameObject[] _effectObjects = new GameObject[4];

        public GameObject _effectObject;
        public GameObject _meteorObject;

        public MeteorType MeteorType { get => _type; }

        private void OnEnable()
        {
            CheckType(_type, _meteorMesh, _effectMesh);
        }

        public void CheckHit(Collision collision = null, Collider collider = null)
        {
            if(collision != null)
            {
                if (collision.transform.gameObject.CompareTag("Plane"))
                {
                    MiniGameController.Instance.AddToPool(this.gameObject);
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
        public void CheckType(MeteorType type, MeshFilter meteor, MeshFilter effect)
        {
            //SetMeshes(type, meteor, effect);
            SetEffect(type);
        }
        //TODO THEIR CHANGES WILL NOT BE EQUAL
        public void SetType()
        {
            int i = Random.Range(0, 4);
            switch (i)
            {
                case 0:
                    _type = MeteorType.classic;
                    break;
                case 1:
                    _type = MeteorType.explosion;
                    break;
                case 2:
                    _type = MeteorType.poison;
                    break;
                case 3:
                    _type = MeteorType.sticky;
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

        private void SetMeshes(MeteorType type, MeshFilter meteor, MeshFilter effect)
        {
            switch (type)
            {
                case MeteorType.classic:
                    meteor.mesh = MiniGameManager.MeteorMeshes.classic;
                    effect.mesh = MiniGameManager.MeteorEffectMeshes.classic;
                    break;
                case MeteorType.explosion:
                    meteor.mesh = MiniGameManager.MeteorMeshes.explosion;
                    effect.mesh = MiniGameManager.MeteorEffectMeshes.explosion;
                    break;
                case MeteorType.poison:
                    meteor.mesh = MiniGameManager.MeteorMeshes.poison;
                    effect.mesh = MiniGameManager.MeteorEffectMeshes.poison;
                    break;
                case MeteorType.sticky:
                    meteor.mesh = MiniGameManager.MeteorMeshes.sticky;
                    effect.mesh = MiniGameManager.MeteorEffectMeshes.sticky;
                    break;
            }
        }
        //TODO
        private void SetEffect(MeteorType type)
        {
            switch (type)
            {
                case MeteorType.classic:
                    _effectObject = _effectObjects[0];
                    break;
                case MeteorType.explosion:
                    _effectObject = _effectObjects[1];
                    break;
                case MeteorType.poison:
                    _effectObject = _effectObjects[2];
                    break;
                case MeteorType.sticky:
                    _effectObject = _effectObjects[3];
                    break;
            }
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