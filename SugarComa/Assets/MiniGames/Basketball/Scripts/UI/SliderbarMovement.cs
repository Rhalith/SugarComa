using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.UI
{
    public class SliderbarMovement : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed;
        [SerializeField] private bool _isStopped;
        [SerializeField] private bool _isLeft;

        public bool IsStopped { set => _isStopped = value; }

        public BarState GetBarPosition()
        {
            switch (_transform.localPosition.y)
            {
                case float n when -0.0012f < n && n < 0.0012f:
                    return BarState.Green;
                case float n when 0.0012f < n && n < 0.004f || -0.004f < n && n < -0.0012:
                    return BarState.Yellow;
                case float n when 0.004f < n && n < 0.0063f || -0.0063f < n && n < -0.004f:
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
                if (_transform.localPosition.y <= -0.0063f) _isLeft = false;
                if (_transform.localPosition.y >= 0.0063f) _isLeft = true;
                if (_isLeft) MoveToLeft();
                else MoveToRight();
            }
        }

        private void MoveToLeft()
        {
            _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y - 0.00005f * _speed, _transform.localPosition.z);
        }

        private void MoveToRight()
        {
            _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y + 0.00005f * _speed, _transform.localPosition.z);
        }
    }

    public enum BarState
    {
        Red,
        Yellow,
        Green
    }
}