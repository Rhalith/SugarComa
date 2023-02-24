using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimation _playerAnimation;
    [SerializeField] private float _speed;
    private bool _isClicking;
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
        _isClicking = true;
        _playerAnimation.StartRunning();
        if(_speed < 200f)_speed+=10;
    }
    public void OnSpeedUp()
    {
        SpeedUpPlayer();
    }
}
