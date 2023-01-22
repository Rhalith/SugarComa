using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderbarMovement : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isStopped;
    [SerializeField] private bool _isLeft;

    public bool IsStopped { set => _isStopped = value; }

    public BarState GetBarPosition()
    {
        switch (_transform.position.x)
        {
            case float n when(-0.213f < n && n < 0.213f):
                return BarState.Green;
            case float n when ((0.213f < n && n < 0.746f) || (-0.746f < n && n < -200)):
                return BarState.Yellow;
            case float n when ((0.746f < n && n < 1.12f) || (-1.12f < n && n < -0.746f)):
                return BarState.Red;
            default:
                return BarState.Red;
        }
    }

    private void FixedUpdate()
    {
        MoveBar();
    }

    private void MoveBar()
    {
        if (!_isStopped)
        {
            if (_transform.position.x <= -1.12f) _isLeft = false;
            if (_transform.position.x >= 1.12f) _isLeft = true;
            if (_isLeft) MoveToLeft();
            else MoveToRight();
        }
    }

    private void MoveToLeft()
    {
        _transform.position = new Vector3(_transform.position.x - (0.005f * _speed), _transform.position.y, _transform.position.z);
    }

    private void MoveToRight()
    {
        _transform.position = new Vector3(_transform.position.x + (0.005f * _speed), _transform.position.y, _transform.position.z);
    }
}

public enum BarState
{
    Red,
    Yellow,
    Green
}
