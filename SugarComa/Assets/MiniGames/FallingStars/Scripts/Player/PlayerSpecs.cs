using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerSpecs : MonoBehaviour
    {
        public int _health;
        public bool _isDead;
        public float _moveSpeed = 5f;
        public float _rotationSpeed = 10f;
    }
}