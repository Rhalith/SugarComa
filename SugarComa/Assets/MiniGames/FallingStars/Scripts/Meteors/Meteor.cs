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
        #region Properties
        public MeteorAction OnMeteorDisable;
        public GameObject MeteorShadow { get => _meteorShadow; }
        public GameObject MeteorObject { get => _meteorObject; }
        public delegate void MeteorAction();

        private MeteorType _type;
        private GameObject _effectObject;
        private readonly GameObject _meteorObject;
        private readonly GameObject _meteorShadow;

        #region SerializeFields
        [SerializeField] private MeshFilter _meteorMesh;
        [SerializeField] private MeshRenderer _meteorRenderer;
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] private GameObject[] _effectObjects = new GameObject[4];
        #endregion
        #endregion

        private void OnEnable()
        {
            CheckType(_type, _meteorMesh, _meteorRenderer);
        }
        private void OnDisable()
        {
            OnMeteorHit(false);
            OnMeteorDisable?.Invoke();
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
        /// <summary>
        /// value => true for enable, value => false for disable
        /// </summary>
        /// <param name="value"></param>
        public void OnMeteorHit(bool value)
        {
            MeteorObject.SetActive(!value);
            MeteorShadow.SetActive(!value);
            _effectObject.SetActive(value);
        }
        //TODO
        private void CheckType(MeteorType type, MeshFilter meteorFilter, MeshRenderer meteorRenderer)
        {
            SetMeshes(type, meteorFilter, meteorRenderer);
            SetEffect(type);
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