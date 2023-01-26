using Assets.MiniGames.Basketball.Scripts.Ball;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Assets.MiniGames.Basketball.Scripts
{
    public class PlayerManager : MonoBehaviour
    {

        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerState _playerState;
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private SliderbarMovement _sliderBarMovement;

        public BallManager BallManager { get => _ballManager; }

        public void OnThrow()
        {
            if (_playerState.Equals(PlayerState.Aiming))
            {
                _ballManager.ChangePlayerState(PlayerState.Shooting);
            }
        }
        public void PlayerAiming()
        {
            _sliderBarMovement.IsStopped = false;
            _playerState = PlayerState.Aiming;
            _playerAnimation.Aim();
        }
        public void PlayerShooting()
        {
            _sliderBarMovement.IsStopped = true;
            _playerState= PlayerState.Shooting;
            _playerAnimation.Throw();
            _playerAnimation.Ball = _ballManager.GetBall();
        }
        public void PlayerWaiting()
        {
            _playerState = PlayerState.Waiting;
        }
    }
}
