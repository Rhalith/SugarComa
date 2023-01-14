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


        public void OnThrow()
        {
            if (_playerState.Equals(PlayerState.Aiming))
            {
                PlayerShooting();
            }
        }
        public void PlayerAiming()
        {
            _sliderBarMovement.IsStopped = false;
            _playerState = PlayerState.Aiming;
        }
        public void PlayerShooting()
        {
            _sliderBarMovement.IsStopped = true;
            _playerState= PlayerState.Shooting;
            _playerAnimation.Throw();
            _ballManager.Balls[0].ThrowBall();//Normally it will invoke at the end of animation of throw. But right now there are no animations. So...
        }
        public void PlayerWaiting()
        {
            _playerState = PlayerState.Waiting;
        }
    }
}
