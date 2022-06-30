using UnityEngine;

public class PathTracker : MonoBehaviour
{
    #region Fields

    private float _t;
    private int _step;
    private int _maxStep;
    private int _toForward;
    private bool _isToForward;
    private bool _isSelector;
    private PlatformSpec _spec;
    private Vector3 _startPosition;
    private int _currentPlatformIndex;
    private Platform[] _path;

    public float speed; // game object movement speed.
    public float rotationSpeed; // game object movement speed.

    [HideInInspector] public bool isMoving; // does the gameobject move.
    #endregion

    #region SerializeFields

    [SerializeField] private Vector3 _currentPosition;
    #endregion

    #region Properties

    /// <summary>
    /// Returns the current platform.
    /// </summary>
    public Platform CurrentPlatform
    {
        get
        {
            // if there is no path
            if (_path == null || _path.Length == 0) return null;

            if (_currentPlatformIndex <= -1) return _path[0];
            else if (_currentPlatformIndex >= _path.Length) return _path[^1];

            return _path[_currentPlatformIndex];
        }
    }
    public int PathLength => _path.Length;

    #endregion

    #region Events

    public delegate void TrackerAction();

    /// <summary>
    /// Invoke when <see cref="CurrentPlatform"/> changed.
    /// </summary>
    public event TrackerAction OnCurrentPlatformChanged;

    /// <summary>
    /// Invoke when the tracker starts
    /// </summary>
    public event TrackerAction OnTrackingStarted;

    /// <summary>
    /// Invoke when the move is over.
    /// </summary>
    public event TrackerAction OnTrackingStopped;
    #endregion

    /// <summary>
    /// Tracking starts on the given path.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="spec"></param>
    /// <param name="maxStep"></param>
    /// <param name="toForward"></param>
    public void StartTracking(Platform[] path, PlatformSpec spec = PlatformSpec.Empty, bool isSelector = false, int maxStep = -1, bool toForward = true)
    {
        // if path exists
        if (path == null || path.Length == 0) return;

        _path = path;
        _step = -1;
        _spec = spec;
        _maxStep = maxStep;
        _isToForward = toForward;
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

        isMoving = true;
        NextPlatform();
        OnTrackingStarted?.Invoke();
    }

    /// <summary>
    /// The movement starts on the last given path.
    /// </summary>
    /// <param name="maxStep"></param>
    /// <param name="toForward"></param>
    public void RestartTracking(int maxStep = -1, bool toForward = true, PlatformSpec specification = PlatformSpec.Empty)
    {
        // if path exists
        if (_path == null || _path.Length == 0) return;

        _step = -1;
        _maxStep = maxStep;
        _isToForward = toForward;
        _toForward = toForward ? 1 : -1;
        _spec = specification;

        isMoving = true;
        NextPlatform();
        OnTrackingStarted?.Invoke();
    }

    private void Update()
    {
        if (!isMoving) return;

        _t += Time.deltaTime * speed;

        if (transform.position != _currentPosition)
        {
            // if object position not equal the current platform position move to position.
            transform.position = Vector3.Lerp(_startPosition, _currentPosition, _t);

            // rotation
            Vector3 movementDirection = (_currentPosition - transform.position).normalized;
            if (movementDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
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
            // platform is selector or current spec equals to the given spec then move is over.
            if (_isSelector || (_currentPlatformIndex > 0 &&
                _currentPlatformIndex < _path.Length - 1 &&
                _spec == _path[_currentPlatformIndex].spec)) condition = false;

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

            OnCurrentPlatformChanged?.Invoke();
        }
        else
        {
            isMoving = false;
            OnTrackingStopped?.Invoke();
        }
    }
}
