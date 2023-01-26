using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.Ball
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private SliderbarMovement _slideBar;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private List<BallMovement> _balls;

        private BallShots _ballShots;

        public List<BallMovement> Balls { get => _balls; }
        public SliderbarMovement SlideBar { get => _slideBar; }
        public BallShots BallShots { get => _ballShots; }

        private void Start()
        {
            _ballShots = new BallShots();
            ChangePlayerState(PlayerState.Aiming);
        }

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

        public BallMovement GetBall()
        {
            //will change
            return _balls[0];
        }
    }
}