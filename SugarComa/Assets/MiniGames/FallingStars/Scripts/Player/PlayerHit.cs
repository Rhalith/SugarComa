using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class PlayerHit : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerManager _currentPlayer;
        [SerializeField] private PlayerAnimation _playerAnimation;
        private List<PlayerManager> _playerManagers = new();
        private bool _punch;

        private void Start()
        {
            _playerMovement.PlayerInput.PlayerInputs.Punch.started += Punch_started;
            _playerMovement.PlayerInput.PlayerInputs.Punch.canceled += Punch_started;
        }
        // TODO animasyona ekle
        private void Punch_started(InputAction.CallbackContext obj)
        {
            print("test");
            _punch = obj.ReadValueAsButton();
            if (_punch)
            {
                _playerAnimation.StartToHit();
                foreach (PlayerManager player in _playerManagers)
                {
                    player.GetHit(_currentPlayer.transform, player.transform);
                }
            }
            else
            {
                _playerAnimation.EndToHit();

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other != _currentPlayer)
            {
                _playerManagers.Add(other.GetComponent<PlayerManager>());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other != _currentPlayer)
            {
                _playerManagers.Remove(other.GetComponent<PlayerManager>());
            }
        }
    }
}