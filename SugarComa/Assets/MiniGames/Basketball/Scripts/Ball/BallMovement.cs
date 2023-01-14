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
            _rigidBody.AddForce(_vector * _velocity, ForceMode.Impulse);
            _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
            _ballManager.ChangePlayerState(PlayerState.Waiting);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                _ballManager.ChangePlayerState(PlayerState.Aiming);
            }
        }

    }
}