using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.ThirdPerson
{
    public class Grappling : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _gunTip;
        [SerializeField] private LayerMask _whatIsGrappleable;
        [SerializeField] private LineRenderer _lineRenderer;

        [Header("Grappling")]
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _delayTime;
        private Vector3 _grapplePoint;

        [Header("Cooldown")]
        [SerializeField] private float _grapplingCoolDown;
        [SerializeField] private float _coolDownTimer;

        public bool grappling;

        private void Update()
        {
            CheckCoolDownTime();
        }
        private void LateUpdate()
        {
            if (grappling)
            {
                _lineRenderer.SetPosition(0, _gunTip.position);
            }
        }

        public void StartGrapple()
        {
            if(_coolDownTimer > 0)
            {
                return;
            }
            grappling = true;

            RaycastHit hit;

            if(Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsGrappleable))
            {
                _grapplePoint = hit.point;

                Invoke(nameof(ExecuteGrapple), _delayTime);
            }
            else
            {
                _grapplePoint = _camera.position + _camera.forward * _maxDistance;

                Invoke(nameof(StopGrapple), _delayTime);
            }
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(1, _grapplePoint);
        }

        private void ExecuteGrapple()
        {

        }

        public void StopGrapple()
        {
            grappling = false;

            _coolDownTimer = _grapplingCoolDown;

            _lineRenderer.enabled = false;
        }

        private void CheckCoolDownTime()
        {
            if(_coolDownTimer > 0)
            {
                _coolDownTimer -= Time.deltaTime;
            }
        }
    }
}