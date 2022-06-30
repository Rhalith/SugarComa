using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Fields
    private bool _moveStart;
    private RouteSelectorDirection _selectorDirection;

    public int goalStep = 5;
    public int maximumStep = 1;
    public bool isDiceRolled;
    public bool isAnimationStopped;
    public bool isRunningAnimation;
    public bool isUserInterfaceActive;
    #endregion

    #region Serialize Fields

    [Header("Status")]
    private int _currentStep;

    [Header("Other Scripts")]
    [SerializeField] private ItemPool _itemPool;
    [SerializeField] private MapCamera _mapCamera;
    [SerializeField] private PathFinder _pathfinder;
    [SerializeField] private PathFollower _pathFollower;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Platform _currentPlatform;
    [SerializeField] private GameController _gameController;
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerAnimation _playerAnimation;
    #endregion

    #region Properties
    
    public int CurrentStep => _currentStep;
    public MapCamera MapCamera { set => _mapCamera = value; }
    public PathFinder PathFinder { set => _pathfinder = value; }
    public Platform CurrentPlatform { get => _currentPlatform; set => _currentPlatform = value; }
    public GameController GameController { get => _gameController; set => _gameController = value; }
    #endregion

    private void FixedUpdate()
    {
        if (_currentStep <= 0 && _playerInput.nextSelectionStepPressed && !isUserInterfaceActive && _playerAnimation._land)
        {
            _currentStep = maximumStep;
            if (!isAnimationStopped) RollDice();
        }

        if (!_currentPlatform.HasSelector && !isUserInterfaceActive && isAnimationStopped && isDiceRolled)
        {
            if (!_pathFollower.isMoving)
            {
                if (!isRunningAnimation) _playerAnimation.StartRunning();
                StartMove();
                if (isAnimationStopped) _playerAnimation.SetIsAnimation();
            }
        }
        else if (!isUserInterfaceActive && _currentPlatform.isSelector && _currentStep > 0)
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
            var path = _pathfinder.ToSelector(_currentPlatform, _currentStep);         
            StartFollowPath(path);
            isDiceRolled = false;
        }
        else if (_playerInput.nextSelectionPressed)
        {
            var path = _pathfinder.ToSelector(_currentPlatform);
            StartFollowPath(path);
            isDiceRolled = false;

        }
        else if (_playerInput.nextGoalPressed)
        {
            var path = _pathfinder.FindBest(_currentPlatform, PlatformSpec.Goal);
            StartFollowPath(path);
            isDiceRolled = false;

        }
        else if (_playerInput.nextGoalStepPressed)
        {
            var path = _pathfinder.FindBest(_currentPlatform, PlatformSpec.Goal, goalStep);
            StartFollowPath(path);
            isDiceRolled = false;

        }
        else if (_playerInput.moveToBackStepPressed)
        {
            _moveStart = true;
            _pathFollower.MoveLastPath(maximumStep, false, PlatformSpec.Goal);
            isDiceRolled = false;
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
        if (isRunningAnimation) _playerAnimation.StopRunning();
        if (_playerInput.selectLeftPressed && !isRunningAnimation) SelectPlatform(RouteSelectorDirection.Left);
        else if (_playerInput.selectRightPressed && !isRunningAnimation) SelectPlatform(RouteSelectorDirection.Right);

        if (_playerInput.applySelectPressed && _selectorDirection != RouteSelectorDirection.None)
        {
            var path = _pathfinder.ToSelector(_currentPlatform, _currentStep, _selectorDirection);
            SelectPlatform(RouteSelectorDirection.None);
            StartFollowPath(path, true);
            _currentPlatform = path[0];
            if (isAnimationStopped && !isRunningAnimation && isDiceRolled)
            {
                _playerAnimation.StartRunning();
                _playerAnimation.SetIsAnimation();
                isDiceRolled = false;
                return;
            }
            if (!isRunningAnimation) _playerAnimation.ContinueRunning();
        }
    }

    public void StartFollowPath(Platform[] path, bool isSelector = false)
    {
        if (path == null || _moveStart) return;

        if (path.Length > 0)
        {
            _moveStart = true;
            if (isSelector)
            {
                _pathFollower.StartFollow(path, PlatformSpec.Goal);
            }
            else
            {
                _pathFollower.StartFollow(path, PlatformSpec.Goal, _currentPlatform.isSelector);
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
                this._currentPlatform = current;
                if(this._currentPlatform.GetPlatformSpec() != PlatformSpec.Goal) _currentStep -= Mathf.Min(_currentStep, _pathFollower.PathLength);
                if (_currentStep <= 0 || !this._currentPlatform.isSelector)
                {
                    _playerAnimation.StopRunning();
                    _playerCollector.CheckCurrentNode(this._currentPlatform);
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
                _currentPlatform.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.greenMaterial);
                _currentPlatform.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial);
                break;
            case RouteSelectorDirection.Right:
                _currentPlatform.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.greenMaterial);
                _currentPlatform.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial);
                break;
            default:
                _currentPlatform.selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial);
                _currentPlatform.selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial);
                break;
        }
    }

    private void RollDice()
    {
        _playerAnimation.RollDice();
        isDiceRolled = true;
        //maximumStep = Random.Range(1, 10);
    }
}
