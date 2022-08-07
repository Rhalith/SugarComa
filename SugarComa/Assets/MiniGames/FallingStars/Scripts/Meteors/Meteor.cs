using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorObjects;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors
{
    public class Meteor : MonoBehaviour
    {
        public delegate void MeteorAction();
        MeteorType _type;
        [SerializeField] MeshFilter _meteorMesh;
        [SerializeField] MeshRenderer _meteorRenderer;
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] GameObject[] _effectObjects = new GameObject[4];

        private GameObject _effectObject;
        public GameObject _meteorObject;
        public GameObject _meteorShadow;

        public MeteorAction OnMeteorDisable;

        public MeteorType MeteorType { get => _type; }

        private void OnEnable()
        {
            CheckType(_type, _meteorMesh, _meteorRenderer);
        }
        private void OnDisable()
        {
            OnMeteorHit(false);
            OnMeteorDisable?.Invoke();
        }
        //TODO
        public void CheckType(MeteorType type, MeshFilter meteorFilter, MeshRenderer meteorRenderer)
        {
            SetMeshes(type, meteorFilter, meteorRenderer);
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
        private void SetMeshes(MeteorType type, MeshFilter meteorFilter, MeshRenderer meteorRenderer)
        {
            switch (type)
            {
                case MeteorType.classic:
                    meteorFilter.mesh = MiniGameManager.MeteorMeshes.classic;
                    break;
                case MeteorType.explosion:
                    meteorFilter.mesh = MiniGameManager.MeteorMeshes.explosion;
                    break;
                case MeteorType.poison:
                    meteorFilter.mesh = MiniGameManager.MeteorMeshes.poison;
                    break;
                case MeteorType.sticky:
                    meteorFilter.mesh = MiniGameManager.MeteorMeshes.sticky;
                    break;
            }
            meteorRenderer.materials = MiniGameManager.MeteorMaterials.meteor;
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
        /// <summary>
        /// value => true for enable, value => false for disable
        /// </summary>
        /// <param name="value"></param>
        public void OnMeteorHit(bool value)
        {
            _meteorObject.SetActive(!value);
            _meteorShadow.SetActive(!value);
            _effectObject.SetActive(value);
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