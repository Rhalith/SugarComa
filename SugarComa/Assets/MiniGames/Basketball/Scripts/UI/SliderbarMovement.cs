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

    private void FixedUpdate()
    {
        MoveBar();
    }

    private void MoveBar()
    {
        if (!_isStopped)
        {
            if (_rectTransform.anchoredPosition3D.x <= -230) _isLeft = false;
            if (_rectTransform.anchoredPosition3D.x >= 230) _isLeft = true;
            if (_isLeft) MoveToLeft();
            else MoveToRight();
        }
    }

    private void MoveToLeft()
    {
        _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x - (1 * _speed), _rectTransform.anchoredPosition3D.y, _rectTransform.anchoredPosition3D.z);
    }

    private void MoveToRight()
    {
        _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x + (1 * _speed), _rectTransform.anchoredPosition3D.y, _rectTransform.anchoredPosition3D.z);
    }
}
