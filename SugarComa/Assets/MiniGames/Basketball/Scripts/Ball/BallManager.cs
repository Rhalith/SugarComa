using Assets.MiniGames.Basketball.Scripts.Player;
using Assets.MiniGames.Basketball.Scripts.UI;
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
            Invoke(nameof(StartGame), 3f);
        }

        private void StartGame()
        {
            _balls[0].PrepareBall();
            _playerManager.PlayerAnimation.Ball = _balls[0];
        }

        public void StartDribbling()
        {
            _playerManager.PlayerAiming();
        }
        public BallMovement GetBall()
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_balls[i].IsReady)
                {
                    _balls[i].PrepareBall();
                    return _balls[i];
                }
            }
            return null;
        }
    }
}