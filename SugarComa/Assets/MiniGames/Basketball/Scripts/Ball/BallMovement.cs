using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.Ball
{
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Vector3 _vector;
        [SerializeField] private float _velocity;

        private void Start()
        {
            _ballManager.ChangePlayerState(PlayerState.Waiting);
        }
        public void ThrowBall()
        {
            _rigidBody.isKinematic = false;
            CheckBarState(_ballManager.SlideBar.GetBarPosition(), _ballManager.BallShots);
        }
        public void ThrowBallVector()
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce(_vector * _velocity, ForceMode.Impulse);
            _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
        }

        public void ResetBall()
        {
            _rigidBody.isKinematic = true;
            transform.position = new Vector3(0, 3.6f, 2.2f);
        }

        private void CheckBarState(BarState barState, BallShots ballShots)
        {
            switch (barState)
            {
                case BarState.Green:
                    _rigidBody.AddForce(ballShots.GetGoal() * _velocity, ForceMode.Impulse);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
                case BarState.Yellow:
                    _rigidBody.AddForce(ballShots.GetAirBall() * _velocity, ForceMode.Impulse);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
                case BarState.Red:
                    _rigidBody.AddForce(ballShots.GetBrick() * _velocity, ForceMode.Impulse);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
            }
        }

    }
}