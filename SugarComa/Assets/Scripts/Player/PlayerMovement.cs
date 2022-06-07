using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int _step = 1;
    [SerializeField] private Platform _current;

    [Header("Diðer Scriptler")]
    [SerializeField] private PathFinder _pathfinder;

    [HideInInspector] private bool _moveStart;
    [HideInInspector] private PathFollower _pathFollower;
    [HideInInspector] private PlayerCollector _playerCollector;
    [HideInInspector] private RouteSelectorDirection _selectorDirection;

    private void Start()
    {
        _playerCollector = GetComponent<PlayerCollector>();
        _pathFollower = GetComponent<PathFollower>();
    }

    private void Update()
    {
        if (!_current.HasSelector)
        {
            if (!_pathFollower.isMoving)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    var path = _pathfinder.ToSelector(_current, _step);
                    StartMoving(path, true);
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    var path = _pathfinder.ToSelector(_current);
                    StartMoving(path, true);
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    var path = _pathfinder.FindBest(_current, PlatformSpecification.Goal);
                    StartMoving(path);
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    var path = _pathfinder.FindBest(_current, PlatformSpecification.Goal, 5);
                    StartMoving(path);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A)) SelectPlatform(RouteSelectorDirection.Left);
            else if (Input.GetKeyDown(KeyCode.D)) SelectPlatform(RouteSelectorDirection.Right);

            if (Input.GetKeyDown(KeyCode.Return) && _selectorDirection != RouteSelectorDirection.None)
            {
                var path = _pathfinder.ToSelector(_current, _step, _selectorDirection);
                SelectPlatform(RouteSelectorDirection.None);
                StartMoving(path, true);
            }
        }

        MoveOver();
    }

    private void StartMoving(Platform[] path, bool increment = false)
    {
        if (path == null || _moveStart) return;

        var currentIndex = increment ? path.Length - 1 : 0;
        if (path.Length > 0)
        {
            _moveStart = true;
            _pathFollower.StartFollow(path, increment);
            _current = path[currentIndex];
        }
    }

    private void MoveOver()
    {
        if (_moveStart && !_pathFollower.isMoving)
        {
            _moveStart = false;
            _playerCollector.CheckCurrentNode(_current);
        }
    }

    private void SelectPlatform(RouteSelectorDirection direction)
    {
        _selectorDirection = direction;

        switch (direction)
        {
            case RouteSelectorDirection.Left:
                _current.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.Instance.selection.greenMaterial);
                _current.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.Instance.selection.redMaterial);
                break;
            case RouteSelectorDirection.Right:
                _current.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.Instance.selection.greenMaterial);
                _current.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.Instance.selection.redMaterial);
                break;
            default:
                _current.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.Instance.selection.redMaterial);
                _current.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.Instance.selection.redMaterial);
                break;
        }
    }
}
