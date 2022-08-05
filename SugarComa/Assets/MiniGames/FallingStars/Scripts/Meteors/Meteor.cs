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
        [SerializeField] MeshFilter _meteorMesh;
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] GameObject[] _effectObjects = new GameObject[4];

        private GameObject _effectObject;
        [SerializeField] MeteorObject _meteorObject;
        [SerializeField] GameObject _meteorShadow;

        public MeteorType MeteorType { get => _type; }

        private void Awake()
        {
            _meteorObject.OnMeteorHit += OnMeteorHit;
        }

        private void OnEnable()
        {
            CheckType(_type, _meteorMesh);
        }
        //TODO
        public void CheckType(MeteorType type, MeshFilter meteor)
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
        private void SetMeshes(MeteorType type, MeshFilter meteor)
        {
            switch (type)
            {
                case MeteorType.classic:
                    meteor.mesh = MiniGameManager.MeteorMeshes.classic;
                    break;
                case MeteorType.explosion:
                    meteor.mesh = MiniGameManager.MeteorMeshes.explosion;
                    break;
                case MeteorType.poison:
                    meteor.mesh = MiniGameManager.MeteorMeshes.poison;
                    break;
                case MeteorType.sticky:
                    meteor.mesh = MiniGameManager.MeteorMeshes.sticky;
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

        private void OnMeteorHit()
        {
            _meteorShadow.SetActive(false);
            _effectObject.SetActive(true);
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