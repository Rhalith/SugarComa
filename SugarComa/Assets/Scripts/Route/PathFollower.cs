using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float speed;

    [HideInInspector] public bool isMoving;

    private float _t;
    private Platform[] _path;
    private int _currentPlatformIndex;
    private Vector3 _currentPosition;
    private Vector3 _startPosition;

    public void StartFollow(Platform[] path)
    {
        if (path == null || path.Length == 0) return;

        _path = path;
        _currentPlatformIndex = -1;

        NextPlatform();

        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        _t += Time.fixedDeltaTime * speed;

        if (transform.position != _currentPosition)
        {
            // if object position not equal the current platform position move to position.
            transform.position = Vector3.Lerp(_startPosition, _currentPosition, _t);
        }
        else
        {
            // if the object position is equal to the current platform position, move to the next platform.
            NextPlatform();
        }
    }

    private void NextPlatform()
    {
        if (_currentPlatformIndex < _path.Length - 1)
        {
            _t = 0;
            _currentPlatformIndex++;
            _startPosition = transform.position;
            _currentPosition = _path[_currentPlatformIndex].position;
            _currentPosition.y = transform.position.y;
        }
        else isMoving = false;
    }
}
