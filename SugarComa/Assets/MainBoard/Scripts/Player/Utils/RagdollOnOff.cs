using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets.MainBoard.Scripts.Player.Movement;

namespace Assets.MainBoard.Scripts.Player.Utils
{
    public class RagdollOnOff : MonoBehaviour
    {
        [SerializeField] CinemachineImpulseSource _impulseSource;
        [SerializeField] Animator _playerAnimator;
        [SerializeField] PlayerMovement _playerMovement;
        [SerializeField] CapsuleCollider _mainCollider;
        [SerializeField] Collider[] _ragDollColliders;
        [SerializeField] Rigidbody[] _ragDollRigidBodies;
        [SerializeField] Transform[] _ragDollTransforms;
        [SerializeField] Rigidbody _currentRigidBody;
        void Awake()
        {
            RagDollOff();
        }

        public void ResetPlayerPosition()
        {
            _ragDollTransforms[2].localPosition = new Vector3(-4.27748774e-11f, 0.00428309385f, 2.75247297e-10f);
            _ragDollTransforms[3].localPosition = new Vector3(7.45337472e-11f, 0.0046511665f, -1.10896591e-10f);
            _ragDollTransforms[4].localPosition = new Vector3(4.27748774e-11f, 0.00428309385f, 2.75247297e-10f);
            _ragDollTransforms[5].localPosition = new Vector3(-7.45337472e-11f, 0.0046511665f, -1.10896591e-10f);
            _ragDollTransforms[6].localPosition = new Vector3(-3.806781e-10f, 0.00465115765f, 3.9763106e-11f);
            _ragDollTransforms[7].localPosition = new Vector3(4.21423452e-10f, 0.00513040042f, -6.98491889e-11f);
            _ragDollTransforms[8].localPosition = new Vector3(3.806781e-10f, 0.00465115765f, 3.9763106e-11f);
            _ragDollTransforms[9].localPosition = new Vector3(-8.03265676e-10f, 0.00513039948f, -2.79396766e-11f);
            _ragDollTransforms[10].localPosition = new Vector3(0, 0.00268863561f, 0);
            _ragDollTransforms[11].localRotation.SetLookRotation(new Vector3(-107.663f, 90, 0));
            _ragDollTransforms[12].localRotation.SetLookRotation(new Vector3(-107.663f, -90, 0));
            _ragDollTransforms[1].localEulerAngles = new Vector3(90, 0, 0);
        }
        public void RagDollOn()
        {
            _playerAnimator.enabled = false;
            _mainCollider.enabled = false;
            foreach (Collider collider in _ragDollColliders)
            {
                collider.enabled = true;
            }

            foreach (Rigidbody rb in _ragDollRigidBodies)
            {
                rb.isKinematic = false;
            }
            _impulseSource.GenerateImpulse();
            StartCoroutine(ForDeath());
            //_currentRigidBody.isKinematic = true;
        }

        void RagDollOff()
        {
            foreach (Collider collider in _ragDollColliders)
            {
                collider.enabled = false;
            }

            foreach (Rigidbody rb in _ragDollRigidBodies)
            {
                rb.isKinematic = true;
            }

            _mainCollider.enabled = true;
            _playerAnimator.enabled = true;
            ResetPlayerPosition();
            //_currentRigidBody.isKinematic = false;
        }

        IEnumerator ForDeath()
        {
            yield return null;
            yield return new WaitForSeconds(2f);
            RagDollOff();
            _playerMovement.OnDeath();
        }
    }
}