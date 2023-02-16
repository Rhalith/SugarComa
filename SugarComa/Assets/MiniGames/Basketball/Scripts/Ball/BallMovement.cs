using Assets.MiniGames.Basketball.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.MiniGames.Basketball.Scripts.Ball
{
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Vector3 _vector;
        [SerializeField] private float _velocity;

        private bool _isReady = false;

        public bool IsReady { get => _isReady; set => _isReady = value; }

        public void ThrowBall()
        {
            _meshRenderer.enabled = true;
            _rigidBody.isKinematic = false;
            CheckBarState(_ballManager.SlideBar.GetBarPosition(), _ballManager.BallShots);
            _isReady = false;
        }
        public void PrepareBall()
        {
            _meshRenderer.enabled = false;
            _rigidBody.isKinematic = true;
            _isReady = true;
            transform.position = new Vector3(-0.1026847f, 4.775045f, 1.953461f);
            _ballManager.StartDribbling();
        }
        [Obsolete("Should be deleted after testing complete.")]
        #region InspectorMethods
        public void ThrowBallVector()
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce(_vector, ForceMode.VelocityChange);
            _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
        }
        public void ResetBall()
        {
            transform.position = new Vector3(-0.1026847f, 4.775045f, 1.953461f);
            _rigidBody.isKinematic = true;
        }
        #endregion
        private void CheckBarState(BarState barState, BallShots ballShots)
        {
            switch (barState)
            {
                case BarState.Green:
                    _rigidBody.AddForce(ballShots.GetGoal(), ForceMode.VelocityChange);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
                case BarState.Yellow:
                    _rigidBody.AddForce(ballShots.GetAirBall(), ForceMode.VelocityChange);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
                case BarState.Red:
                    _rigidBody.AddForce(ballShots.GetBrick(), ForceMode.VelocityChange);
                    _rigidBody.AddTorque(new Vector3(0, 0, 1) * _velocity, ForceMode.Impulse);
                    break;
            }
        }
    }
}