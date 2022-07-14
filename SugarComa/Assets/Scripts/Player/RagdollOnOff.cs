using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] CapsuleCollider _mainCollider;
    [SerializeField] Collider[] _ragDollColliders;
    [SerializeField] Rigidbody[] _ragDollRigidBodies;

    void Awake()
    {
        RagDollOff();
    }

    void RagDollOn()
    {
        foreach (Collider collider in _ragDollColliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rb in _ragDollRigidBodies)
        {
            rb.isKinematic = false;
        }

        _mainCollider.enabled = false;
        _playerAnimator.enabled = false;
    }

    void RagDollOff()
    {
        foreach(Collider collider in _ragDollColliders)
        {
            collider.enabled = false;
        }

        foreach(Rigidbody rb in _ragDollRigidBodies)
        {
            rb.isKinematic = true;
        }

        _mainCollider.enabled = true;
        _playerAnimator.enabled = true;
    }
}
