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
        public MeteorType Type { get => _type; private set => _type = value; }

        public delegate void MeteorAction();

        private MeteorType _type;
        private GameObject _meteorObject;
        private GameObject _effectObject;

        #region SerializeFields
        [SerializeField] private GameObject _meteorShadow;
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] private GameObject[] _meteorObjects = new GameObject[4];
        [Tooltip("Order -> Classic -> Explosion -> Poison -> Sticky")]
        [SerializeField] private GameObject[] _effectObjects = new GameObject[4];
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
            _effectObject.SetActive(value);
        }
        private void ResetMeteor(bool value)
        {
            MeteorShadow.SetActive(value);
            _effectObject.SetActive(!value);
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