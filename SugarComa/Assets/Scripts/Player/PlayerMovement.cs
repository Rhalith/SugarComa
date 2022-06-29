using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int maximumStep = 1;
    public int goalStep = 5;
    public bool isAnimationStopped;
    public bool isRunningAnimation;

    [SerializeField] public Platform _current;
    [SerializeField] private PathFinder _pathfinder;
    public GameController _gameController;
    [Header("Status")]
    [SerializeField] private int _currentStep;

    private bool _moveStart;
    [SerializeField] private PathFollower _pathFollower;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private MapCamera _mapCamera;
    [SerializeField] private ItemPool _itemPool;
    [SerializeField] private PlayerAnimation _playerAnimation;
    private RouteSelectorDirection _selectorDirection;

    public bool isUserInterfaceActive;

    private void FixedUpdate()
    {
        if (_currentStep <= 0 && _playerInput.nextSelectionStepPressed && !isUserInterfaceActive && _playerAnimation._land)
        {
            _currentStep = maximumStep; 
            if(!isAnimationStopped) _playerAnimation.RollDice();
        } 

        if (!_current.HasSelector && !isUserInterfaceActive && isAnimationStopped )
        {
            if (!_pathFollower.isMoving)
            { 
                if (!isRunningAnimation) _playerAnimation.StartRunning();
                StartMove();
                if (isAnimationStopped) _playerAnimation.SetIsAnimation();
            }
        }
        else if (!isUserInterfaceActive)
        {
            ProcessSelect();
        }
        ProcessUI();
        MoveOver();
    }

    private void StartMove()
    {
        if (_playerInput.nextSelectionStepPressed || isRunningAnimation)
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
        else if (_playerInput.closeUI)
        {
            _playerCollector.AddItem();
        }
    }

    private void ProcessUI()
    {
        if (_playerInput.openInventory)
        {
            _playerInventory.OpenInventory();
            isUserInterfaceActive = true;
        }
        else if (_playerInput.closeUI && !ItemPool._isItemUsing)
        {
            _playerInventory.CloseInventory();
            _mapCamera.SetCameraPriority(_mapCamera._camera, _mapCamera._mainCamera.Priority - 1, true);
            isUserInterfaceActive = false;
        }
        else if (_playerInput.openMap)
        {
            _mapCamera.SetCameraPriority(_mapCamera._camera, _mapCamera._mainCamera.Priority + 1);
            isUserInterfaceActive = true;
        }
        else if (_playerInput.closeUI && ItemPool._isItemUsing)
        {
            _itemPool.CloseItem();
            isUserInterfaceActive = false;
        }
    }
    private void ProcessSelect()
    {
        if (isRunningAnimation && _current.isSelector) _playerAnimation.StopRunning();
        if (_playerInput.selectLeftPressed) SelectPlatform(RouteSelectorDirection.Left);
        else if (_playerInput.selectRightPressed) SelectPlatform(RouteSelectorDirection.Right);

        if (_playerInput.applySelectPressed && _selectorDirection != RouteSelectorDirection.None)
        {
            
            var path = _pathfinder.ToSelector(_current, _currentStep, _selectorDirection);
            SelectPlatform(RouteSelectorDirection.None);
            StartFollowPath(path, true);
            if (!isRunningAnimation) _playerAnimation.ContinueRunning();
        }
    }

    private void StartFollowPath(Platform[] path, bool isSelector = false)
    {
        if (path == null || _moveStart) return;

        if (path.Length > 0)
        {
            _moveStart = true;
            if (isSelector)
            {
                _pathFollower.StartFollow(path, PlatformSpecification.Goal);
            }
            else
            {
                _pathFollower.StartFollow(path, PlatformSpecification.Goal, _current.isSelector);
            }
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
                if(_currentStep <= 0 || !_current.isSelector)
                {
                    _playerAnimation.StopRunning();
                    _playerCollector.CheckCurrentNode(_current);
                }
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

    public void SetPathFinder(PathFinder pathfinder)
    {
        _pathfinder = pathfinder;
    }

    public void SetCurrentPlatform(Platform platform)
    {
        _current = platform;
    }

    public void SetMapCamera(MapCamera mapCamera)
    {
        _mapCamera = mapCamera;
    }

    public void SetGameController(GameController gameController)
    {
        _gameController = gameController;
    }
}
