using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float speed; // game object movement speed.
    public float rotationSpeed; // game object movement speed.

    public int PathLength => _path.Length;

    [HideInInspector] public bool isMoving; // does the gameobject move.

    private float _t;
    private Platform[] _path;
    private int _currentPlatformIndex;
    [SerializeField] private Vector3 _currentPosition;
    private Vector3 _startPosition;
    private int _step;
    private int _maxStep;
    private int _toForward;
    private bool _isToForward;
    private bool _isSelector;
    private PlatformSpec _specification;

    [SerializeField] GameObject player;
    /// <summary>
    /// The movement starts on the given path.
    /// </summary>
    /// <param name="path"></param>
    public void StartFollow(Platform[] path, PlatformSpec specification = PlatformSpec.Empty, bool isSelector = false, int maxStep = -1, bool toForward = true)
    {
        // if path exists
        if (path == null || path.Length == 0) return;

        _path = path;
        _step = -1;
        _maxStep = maxStep;
        _isToForward = toForward;
        _specification = specification;
        _isSelector = isSelector;
        if (toForward)
        {
            _toForward = 1;
            _currentPlatformIndex = -1;
        }
        else
        {
            _toForward = -1;
            _currentPlatformIndex = _path.Length;
        }

        NextPlatform();

        isMoving = true;
    }

    /// <summary>
    /// The movement starts on the last given path.
    /// </summary>
    /// <param name="maxStep"></param>
    /// <param name="toForward"></param>
    public void MoveLastPath(int maxStep = -1, bool toForward = true, PlatformSpec specification = PlatformSpec.Empty)
    {
        // if path exists
        if (_path == null || _path.Length == 0) return;

        _step = -1;
        _maxStep = maxStep;
        _isToForward = toForward;
        _toForward = toForward ? 1 : -1;
        _specification = specification;

        NextPlatform();

        isMoving = true;
    }

    /// <summary>
    /// Returns the current platform.
    /// </summary>
    public Platform GetCurrentPlatform()
    {
        // if there is no path
        if (_path == null || _path.Length == 0) return null;

        if (_currentPlatformIndex <= -1) return _path[0];
        else if (_currentPlatformIndex >= _path.Length) return _path[^1];

        return _path[_currentPlatformIndex];
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        _t += Time.fixedDeltaTime * speed;

        if (transform.position != _currentPosition)
        {
            // if object position not equal the current platform position move to position.
            transform.position = Vector3.Lerp(_startPosition, _currentPosition, _t);

            // rotation
            Vector3 movementDirection = (_currentPosition - transform.position).normalized;
            if (movementDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            // if the object position is equal to the current platform position, move to the next platform.
            NextPlatform();
        }
    }

    private void NextPlatform()
    {
        bool condition = _isToForward ? _currentPlatformIndex < _path.Length - 1 : _currentPlatformIndex > 0;

        if (condition)
        {
            if (_isSelector || _currentPlatformIndex > 0 &&
                _currentPlatformIndex < _path.Length - 1 &&
                _specification == _path[_currentPlatformIndex].specification) condition = false;

            // if the step greater than maximum step, stop the movement.
            if (_maxStep != -1) condition = condition && _step < _maxStep;
        }


        if (condition)
        {
            _t = 0;
            _step++;
            _currentPlatformIndex += _toForward;
            _startPosition = transform.position;
            _currentPosition = _path[_currentPlatformIndex].position;
            _currentPosition.y = transform.position.y;
        }
        else isMoving = false;
    }
}
