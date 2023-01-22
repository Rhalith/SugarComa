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
        public void ThrowBall()
        {
            _rigidBody.isKinematic = false;
            _ballManager.ChangePlayerState(PlayerState.Waiting);
            CheckBarState(_ballManager.SlideBar.GetBarPosition());
        }

        public void ResetBall()
        {
            _rigidBody.isKinematic = true;
            transform.position = new Vector3(0, 2, 1);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                _ballManager.ChangePlayerState(PlayerState.Aiming);
            }
        }

        private void CheckBarState(BarState barState)
        {
            switch (barState)
            {
                case BarState.Green:
                    _rigidBody.AddForce(_vector * _velocity, ForceMode.Impulse);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
                case BarState.Yellow:
                    _rigidBody.AddForce(_vector * _velocity * 1.2f, ForceMode.Impulse);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
                case BarState.Red:
                    _rigidBody.AddForce(_vector * _velocity * 1.4f, ForceMode.Impulse);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
            }
        }

    }
}