using Assets.MiniGames.Basketball.Scripts.Ball;
using Assets.MiniGames.Basketball.Scripts.UI;
using UnityEngine;


namespace Assets.MiniGames.Basketball.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {

        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerState _playerState;
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private SliderbarMovement _sliderBarMovement;

        public BallManager BallManager { get => _ballManager; }
        public PlayerAnimation PlayerAnimation { get => _playerAnimation; }

        public void OnThrow()
        {
            if (_playerState.Equals(PlayerState.Aiming))
            {
                PlayerShooting();
            }
        }
        public void PlayerAiming()
        {
            _playerState = PlayerState.Aiming;
            _sliderBarMovement.IsStopped = false;
            _playerAnimation.Aim();
        }
        public void PlayerShooting()
        {
            BallMovement ballMovement = _playerAnimation.Ball;
            if (ballMovement != null)
            {
                _playerState = PlayerState.Shooting;
                _sliderBarMovement.IsStopped = true;
                _playerAnimation.Throw();
            }
        }

    }
}
