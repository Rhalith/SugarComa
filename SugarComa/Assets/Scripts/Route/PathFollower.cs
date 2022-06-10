using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float speed;

    [HideInInspector] public bool isMoving;

    [HideInInspector] private float _t;
    [HideInInspector] private Platform[] _path;
    [HideInInspector] private int _currentPlatformIndex;
    [HideInInspector] private Vector3 _currentPosition;
    [HideInInspector] private Vector3 _startPosition;

    public void StartFollow(Platform[] path)
    {
        if (path == null || path.Length == 0) return;

        _path = path;
        _currentPlatformIndex = -1;

        NextPlatform();

        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        _t += Time.deltaTime * speed;

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
