using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int maximumStep = 1;
    public int goalStep = 5;

    [SerializeField] public Platform _current;
    [SerializeField] private PathFinder _pathfinder;

    [Header("Status")]
    [SerializeField] private int _currentStep;

    private bool _moveStart;
    [SerializeField] private PathFollower _pathFollower;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private PlayerInventory _playerInventory;
    private RouteSelectorDirection _selectorDirection;

    private void FixedUpdate()
    {
        if (_currentStep <= 0 && _playerInput.nextSelectionStepPressed) _currentStep = maximumStep;

        if (!_current.HasSelector)
        {
            if (!_pathFollower.isMoving) StartMove();
        }
        else
        {
            ProcessSelect();
        }

        MoveOver();
    }

    private void StartMove()
    {
        if (_playerInput.nextSelectionStepPressed)
        {
            var path = _pathfinder.ToSelector(_current, _currentStep);
            StartFollowPath(path);
        }
        else if (_playerInput.nextSelectionPressed)
        {
            var path = _pathfinder.ToSelector(_current);
            StartFollowPath(path);
        }
        else if (_playerInput.nextGoalPressed)
        {
            var path = _pathfinder.FindBest(_current, PlatformSpecification.Goal);
            StartFollowPath(path);
        }
        else if (_playerInput.nextGoalStepPressed)
        {
            var path = _pathfinder.FindBest(_current, PlatformSpecification.Goal, goalStep);
            StartFollowPath(path);
        }
        else if (_playerInput.moveToBackStepPressed)
        {
            _moveStart = true;
            _pathFollower.MoveLastPath(maximumStep, false, PlatformSpecification.Goal);
        }
        //just for open inventory, if you think we should declare new method, you can do.
        else if (_playerInput.openInventory)
        {
            _playerInventory.OpenInventory();
        }
        else if (_playerInput.closeUI)
        {
            _playerInventory.CloseInventory();
        }
    }

    private void ProcessSelect()
    {
        if (_playerInput.selectLeftPressed) SelectPlatform(RouteSelectorDirection.Left);
        else if (_playerInput.selectRightPressed) SelectPlatform(RouteSelectorDirection.Right);

        if (_playerInput.applySelectPressed && _selectorDirection != RouteSelectorDirection.None)
        {
            var path = _pathfinder.ToSelector(_current, _currentStep, _selectorDirection);
            SelectPlatform(RouteSelectorDirection.None);
            StartFollowPath(path);
        }
    }

    private void StartFollowPath(Platform[] path)
    {
        if (path == null || _moveStart) return;

        if (path.Length > 0)
        {
            _moveStart = true;
            _pathFollower.StartFollow(path, PlatformSpecification.Goal);
        }
    }

    private void MoveOver()
    {
        if (_moveStart && !_pathFollower.isMoving)
        {
            _moveStart = false;
            var current = _pathFollower.GetCurrentPlatform();
            if (current != null)
            {
                _current = current;
                _currentStep -= Mathf.Min(_currentStep, _pathFollower.PathLength);
                _playerCollector.CheckCurrentNode(_current);
            } 
        }
    }

    private void SelectPlatform(RouteSelectorDirection direction)
    {
        _selectorDirection = direction;

        switch (direction)
        {
            case RouteSelectorDirection.Left:
                _current.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.greenMaterial);
                _current.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial);
                break;
            case RouteSelectorDirection.Right:
                _current.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.greenMaterial);
                _current.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial);
                break;
            default:
                _current.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial);
                _current.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial);
                break;
        }
    }
}
