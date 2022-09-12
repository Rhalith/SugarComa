using Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects;
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
        public MeteorType Type { get => _type; private set => _type = value; }

        public delegate void MeteorAction();

        private MeteorType _type;
        private GameObject _meteorObject;
        private GameObject _effectObject;
        private GameObject _vfxObject = null;

        #region SerializeFields
        [SerializeField] private GameObject _meteorShadow;
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] private GameObject[] _meteorObjects = new GameObject[4];
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] private GameObject[] _effectObjects = new GameObject[4];
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] private GameObject[] _vfxObjects = new GameObject[4];
        #endregion
        #endregion

        //private void Awake()
        //{
        //    _firstPosition.position = transform.position;
        //}

        //private void OnEnable()
        //{
        //    CheckType(Type, _meteorMesh, _meteorRenderer);
        //}
        private void OnDisable()
        {
            ResetMeteor(true);
            OnMeteorDisable?.Invoke();
        }
        //TODO THEIR CHANGES WILL NOT BE EQUAL
        public void SetType()
        {
            int i = Random.Range(0, 4);
            switch (i)
            {
                case 0:
                    Type = MeteorType.classic;
                    break;
                case 1:
                    Type = MeteorType.explosion;
                    break;
                case 2:
                    Type = MeteorType.poison;
                    break;
                case 3:
                    Type = MeteorType.sticky;
                    break;
            }
            CheckType(Type);
        }
        /// <summary>
        /// value => true for enable, value => false for disable
        /// </summary>
        /// <param name="value"></param>
        public void OnMeteorHit(bool value)
        {
            MeteorObject.SetActive(!value);
            MeteorShadow.SetActive(!value);
            if (!_effectObject.Equals(_effectObjects[0]))_effectObject.SetActive(value);
            if (_vfxObject != null) _vfxObject.SetActive(value);
        }

        public void SetObjectMaterial(MeteorType type, Material material)
        {
            switch (type)
            {
                case MeteorType.classic:
                    Material[] classicMaterials = _meteorObjects[0].GetComponent<Renderer>().materials;
                    classicMaterials[2] = material;
                    _meteorObjects[0].GetComponent<Renderer>().materials = classicMaterials;
                    _meteorObjects[0].GetComponent<MeteorObject>().FlameMaterial = material;
                    break;
                case MeteorType.explosion:
                    Material[] explosionMaterials = _meteorObjects[1].GetComponent<Renderer>().materials;
                    explosionMaterials[1] = material;
                    _meteorObjects[1].GetComponent<Renderer>().materials = explosionMaterials;
                    _meteorObjects[1].GetComponent<MeteorObject>().FlameMaterial = material;
                    break;
                case MeteorType.poison:
                    Material[] poisonMaterials = _meteorObjects[2].GetComponent<Renderer>().materials;
                    poisonMaterials[1] = material;
                    _meteorObjects[2].GetComponent<Renderer>().materials = poisonMaterials;
                    _meteorObjects[2].GetComponent<MeteorObject>().FlameMaterial = material;
                    break;
                case MeteorType.sticky:
                    Material[] stickyMaterials = _meteorObjects[3].GetComponent<Renderer>().materials;
                    stickyMaterials[1] = material;
                    _meteorObjects[3].GetComponent<Renderer>().materials = stickyMaterials;
                    _meteorObjects[3].GetComponent<MeteorObject>().FlameMaterial = material;
                    break;
            }
        }

        public void SetEffectMaterial(MeteorType type, Material classic, Material effectOne = null, Material effectTwo = null, Material effectThree = null)
        {
            switch (type)
            {
                case MeteorType.classic:
                    _effectObjects[0].GetComponent<Renderer>().material = classic;
                    break;
                case MeteorType.explosion:
                    _effectObjects[1].GetComponent<ExplosionMeteor>().MiniEffects[0].GetComponent<Renderer>().material = effectOne;
                    _effectObjects[1].GetComponent<ExplosionMeteor>().MiniEffects[1].GetComponent<Renderer>().material = effectTwo;
                    _effectObjects[1].GetComponent<ExplosionMeteor>().MiniEffects[2].GetComponent<Renderer>().material = effectThree;
                    break;
            }
        }
        private void ResetMeteor(bool value)
        {
            MeteorShadow.SetActive(value);
            _effectObject.SetActive(!value);
            if(_vfxObject != null)_vfxObject.SetActive(!value);
        }
        private void CheckType(MeteorType type)
        {
            SetEffect(type);
            SetObject(type);
        }

        private void SetEffect(MeteorType type)
        {
            switch (type)
            {
                case MeteorType.classic:
                    _effectObject = _effectObjects[0];
                    _vfxObject = _vfxObjects[0];
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
        private void SetObject(MeteorType type)
        {
            switch (type)
            {
                case MeteorType.classic:
                    _meteorObject = _meteorObjects[0];
                    break;
                case MeteorType.explosion:
                    _meteorObject = _meteorObjects[1];
                    break;
                case MeteorType.poison:
                    _meteorObject = _meteorObjects[2];
                    break;
                case MeteorType.sticky:
                    _meteorObject = _meteorObjects[3];
                    break;
            }
            _meteorObject.SetActive(true);
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