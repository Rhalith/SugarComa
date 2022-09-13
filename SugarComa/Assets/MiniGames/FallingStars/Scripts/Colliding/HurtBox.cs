using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Colliding
{
    public class HurtBox : MonoBehaviour, IHurtBox
    {
        [SerializeField] private bool _active = true;
        [SerializeField] private GameObject _owner = null;
        [SerializeField] private HurtBoxType _hurtBoxType;
        private IHurtResponder _hurtResponder;
        public bool Active { get => _active; }

        public GameObject Owner { get => _owner; }

        public Transform Transform { get => transform; }
        public HurtBoxType Type { get => _hurtBoxType; } 
        public IHurtResponder HurtResponder { get => _hurtResponder; set => _hurtResponder = value; }


        public bool CheckHit(HitData data)
        {
            if(_hurtResponder == null)
            {
                Debug.Log("Responder does not exists");
            }
            return true;
        }
    }
}
