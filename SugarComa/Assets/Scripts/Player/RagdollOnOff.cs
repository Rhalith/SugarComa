using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] CapsuleCollider _mainCollider;
    [SerializeField] Collider[] _ragDollColliders;
    [SerializeField] Rigidbody[] _ragDollRigidBodies;
    [SerializeField] GameObject currentBoxGloves;
    [SerializeField] Rigidbody _currentRigidBody;

    void Awake()
    {
        RagDollOff();
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    print(other.gameObject.name);
    //    if (other.gameObject.CompareTag("GlovesHit")) RagDollOn();
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject != currentBoxGloves)
    //    {
    //        print(collision.gameObject.name);
    //        if (collision.gameObject.CompareTag("GlovesHit")) RagDollOn();
    //    }
    //}

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
        _currentRigidBody.isKinematic = true;
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
        _currentRigidBody.isKinematic = false;
    }
}
