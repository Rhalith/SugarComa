using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.Ball
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private List<BallMovement> _balls;

        public List<BallMovement> Balls { get => _balls; }

        public void ChangePlayerState(PlayerState playerState)
        {
            if (playerState.Equals(PlayerState.Aiming))
            {
                _playerManager.PlayerAiming();
            }
            else if (playerState.Equals(PlayerState.Shooting))
            {
                _playerManager.PlayerShooting();
            }
            else
            {
                _playerManager.PlayerWaiting();
            }
        }
    }
}