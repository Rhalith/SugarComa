using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private GameObject _text;
        [SerializeField] private GameObject _player;
        private Vector3 _position;

        private void Start()
        {
            _position = _player.transform.position;
        }
        public void KillPlayer()
        {
            gameObject.SetActive(false);
            _text.SetActive(true);
        }
        public void SpawnPlayer()
        {
            _player.transform.position = _position;
            gameObject.SetActive(true);
            _text.SetActive(false);
        }
    }
}