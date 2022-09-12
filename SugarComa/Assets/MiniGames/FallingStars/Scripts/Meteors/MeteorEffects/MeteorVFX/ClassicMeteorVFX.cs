using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorEffects.MeteorVFX
{
    public class ClassicMeteorVFX : MonoBehaviour
    {
        [SerializeField] private GameObject _meteorEffect;
        [SerializeField] private Material _materialLeft;
        [SerializeField] private Material _materialRight;
        [SerializeField] private string _refID;
        public void ActivateEffect(int i)
        {
            if(i == 0)
            {
                _meteorEffect.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
                _materialLeft.SetFloat(_refID, 0);
                _materialRight.SetFloat(_refID, 0);
            }
        }

        public void ActivateMaterials(float x)
        {
            float a = Random.Range(0, 5);
            _materialLeft.SetFloat(_refID, a);
            _materialRight.SetFloat(_refID, a);
            a += x;
            _materialLeft.DOFloat(a, _refID, 1f);
            _materialRight.DOFloat(a, _refID, 1f);
        }
    }
}