using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimation _playerAnimation;
    [SerializeField] private float _speed;
    private bool _isLeft;
    private bool _isFirst = true;
    private void Start()
    {
        _playerAnimation = gameObject.GetComponent<PlayerAnimation>();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        SlowDownPlayer();
        _playerAnimation.ChangeAnimatorSpeed(_speed);
        gameObject.transform.position += new Vector3(0,0,0.1f * _speed * Time.deltaTime);
    }
    private void SlowDownPlayer()
    {
        _speed--;
        if (_speed < 0)
        {
            _playerAnimation.StopRunning();
            _speed = 0;
        }
    }
    private void SpeedUpPlayer()
    {
        _playerAnimation.StartRunning();
        if(_speed < 200f)_speed+=10;
    }
    public void OnSpeedUp(InputValue value)
    {
        CheckKey(value);
    }

    private void CheckKey(InputValue value)
    {
        float input = value.Get<float>();
        if(!_isLeft && input < 0)
        {
            _isLeft = true;
            SpeedUpPlayer();
        }
        else if (_isLeft && input > 0)
        {
            _isLeft = false;
            SpeedUpPlayer();
        }
        //This is only for if player starts with key "D"
        if (_isFirst && !_isLeft && input > 0)
        {
            _isFirst = false;
            _isLeft = false;
            SpeedUpPlayer();
        }
    }
}
