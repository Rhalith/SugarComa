using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderbarMovement : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isStopped;
    [SerializeField] private bool _isLeft;

    public bool IsStopped { set => _isStopped = value; }

    public BarState GetBarPosition()
    {
        switch (_rectTransform.anchoredPosition3D.x)
        {
            case float n when(-200 < n && n < 200):
                return BarState.Green;
            case float n when ((200 < n && n < 700) || (-700 < n && n < -200)):
                return BarState.Yellow;
            case float n when ((700 < n && n < 1050) || (-1050 < n && n < -700)):
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
            if (_rectTransform.anchoredPosition3D.x <= -1050) _isLeft = false;
            if (_rectTransform.anchoredPosition3D.x >= 1050) _isLeft = true;
            if (_isLeft) MoveToLeft();
            else MoveToRight();
        }
    }

    private void MoveToLeft()
    {
        _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x - (10 * _speed), _rectTransform.anchoredPosition3D.y, _rectTransform.anchoredPosition3D.z);
    }

    private void MoveToRight()
    {
        _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x + (10 * _speed), _rectTransform.anchoredPosition3D.y, _rectTransform.anchoredPosition3D.z);
    }
}

public enum BarState
{
    Red,
    Yellow,
    Green
}
