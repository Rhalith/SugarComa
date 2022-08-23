using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Utils
{
    public class LavaFlow : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private MiniGameManager _miniGameManager;
        [SerializeField] private float _endValue;
        void Start()
        {
            _material.SetFloat("_Flow", 0);
            _material.DOFloat(_endValue, "_Flow", _miniGameManager.GameTime);
        }
    }
}