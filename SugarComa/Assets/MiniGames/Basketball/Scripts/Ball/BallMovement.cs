using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;
    [SerializeField] Vector3 _vector;
    [SerializeField] float _velocity;
    void Start()
    {
        _rigidBody.AddForce(_vector * _velocity, ForceMode.Impulse);
    }

}
