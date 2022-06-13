using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float speed; // game object movement speed.

    [HideInInspector] public bool isMoving; // does the gameobject move.

    private float _t;
    private Platform[] _path;
    private int _currentPlatformIndex;
    private Vector3 _currentPosition;
    private Vector3 _startPosition;
    private int _step;
    private int _maxStep;
    private int _increment;
    private bool _isIncrement;

    /// <summary>
    /// Movement goes on the given path.
    /// </summary>
    /// <param name="path"></param>
    public void StartFollow(Platform[] path, int maxStep = -1, bool increment = true)
    {
        // if path exists
        if (path == null || path.Length == 0) return;

        _path = path;

        _step = -1;
        _maxStep = maxStep;
        _isIncrement = increment;
        if (increment)
        {
            _increment = 1;
            _currentPlatformIndex = -1;
        }
        else
        {
            _increment = -1;
            _currentPlatformIndex = path.Length;
        }

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
        bool condition = _isIncrement ? _currentPlatformIndex < _path.Length - 1 : _currentPlatformIndex > 0;

        // if the step greater than maximum step, stop the movement.
        if (_maxStep != -1) condition = condition && _step < _maxStep;

        if (condition)
        {
            _t = 0;
            _step++;
            _currentPlatformIndex += _increment;
            _startPosition = transform.position;
            _currentPosition = _path[_currentPlatformIndex].position;
            _currentPosition.y = transform.position.y;
        }
        else isMoving = false;
    }
}
