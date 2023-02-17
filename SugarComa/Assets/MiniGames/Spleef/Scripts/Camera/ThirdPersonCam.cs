using Assets.MiniGames.HoleInTheWall.Scripts.Player.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerObj;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _rotationSpeed;

    [SerializeField] private PlayerMovement _playerMovement;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        _orientation.forward = viewDir.normalized;

        Vector3 inputDir = _orientation.forward * _playerMovement.Movement.y + _orientation.right * _playerMovement.Movement.x;

        if(inputDir != Vector3.zero) 
        { 
            _playerObj.forward = Vector3.Slerp(_playerObj.forward, inputDir.normalized, Time.deltaTime * _rotationSpeed);
        }

    }
}
