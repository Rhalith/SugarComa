using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Fields

    private RouteSelectorDirection _selectorDirection;
    #endregion

    #region Serialize Fields

    [Header("Status")]
    [SerializeField]  private int _currentStep;
    public int goalStep = 5;
    public int maximumStep = 1;
    public bool isDiceRolled;
    //public bool isAnimationStopped;
    //public bool isRunningAnimation;
    public bool isUserInterfaceActive;

    [Header("Other Scripts")]
    [SerializeField] private ItemPool _itemPool;
    [SerializeField] private MapCamera _mapCamera;
    [SerializeField] private PathFinder _pathFinder;
    [SerializeField] private PathTracker _pathTracker;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Platform _currentPlatform;
    [SerializeField] private GameController _gameController;
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private GobletSelection _gobletSelection;
    #endregion

    #region Properties

    public int CurrentStep => _currentStep;
    public MapCamera MapCamera { set => _mapCamera = value; }
    public PathFinder PathFinder { set => _pathFinder = value; }
    public Platform CurrentPlatform { get => _currentPlatform; set => _currentPlatform = value; }
    public GameController GameController { get => _gameController; set => _gameController = value; }
    #endregion

    private void Start()
    {
        _gameController.ChangeText();
        _pathTracker.OnTrackingStarted += OnTrackingStarted;
        _pathTracker.OnCurrentPlatformChanged += OnCurrentPlatformChanged;
        _pathTracker.OnTrackingStopped += OnTrackingStopped;

        // TODO
        _gobletSelection.OnTakeIt += GobletSelection_OnTakeIt;
        _gobletSelection.OnLeaveIt += GobletSelection_OnLeaveIt;
    }

    private void Update()
    {
        if (!isUserInterfaceActive && _playerAnimation.IsIdle)
        {
            if (_currentStep <= 0 && _playerInput.nextSelectionStepPressed)
            {
                _currentStep = maximumStep;
                RollDice();
            }
            else if (!_currentPlatform.HasSelector && isDiceRolled && !_pathTracker.isMoving)
            {
                StartMove();
            }
            else if (_currentStep > 0)
            {
                ProcessSelect();
            }
        }
        ProcessUI();
    }

    private void StartMove()
    {
        if (_playerInput.nextSelectionStepPressed || !_playerAnimation.IsJumping) // space
        {
            _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep), PlatformSpec.Goal, _currentPlatform.HasSelector);
        }
        else if (_playerInput.nextSelectionPressed) // X
        {
            _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform), PlatformSpec.Goal, _currentPlatform.HasSelector);

        }
        else if (_playerInput.nextGoalPressed) // C
        {
            _pathTracker.StartTracking(_pathFinder.FindBest(_currentPlatform, PlatformSpec.Goal), PlatformSpec.Goal, _currentPlatform.HasSelector);
        }
        else if (_playerInput.nextGoalStepPressed) // V
        {
            _pathTracker.StartTracking(_pathFinder.FindBest(_currentPlatform, PlatformSpec.Goal, goalStep), PlatformSpec.Goal, _currentPlatform.HasSelector);
        }
        else if (_playerInput.moveToBackStepPressed) // B
        {
            _pathTracker.RestartTracking(maximumStep, false, PlatformSpec.Goal);
        }
        isDiceRolled = false;
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
        if (_playerInput.selectLeftPressed) SelectPlatform(RouteSelectorDirection.Left);
        else if (_playerInput.selectRightPressed) SelectPlatform(RouteSelectorDirection.Right);

        if (_playerInput.applySelectPressed && _selectorDirection != RouteSelectorDirection.None)
        {
            RouteSelectorDirection temp = _selectorDirection;
            SelectPlatform(RouteSelectorDirection.None);
            _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep, temp), PlatformSpec.Goal);
        }
    }

    private void GobletSelection_OnLeaveIt()
    {
        _pathTracker.StartTracking(_pathFinder.ToSelector(_currentPlatform, _currentStep), PlatformSpec.Goal);
    }

    private void GobletSelection_OnTakeIt()
    {
        _currentPlatform.ResetSpec();
    }

    private void OnTrackingStarted()
    {
        _playerAnimation.StartRunning();
    }

    private void OnCurrentPlatformChanged()
    {
        var current = _pathTracker.CurrentPlatform;
        if (current != null)
        {
            _currentPlatform = current;
            _currentStep -= 1;
        }
    }

    private void OnTrackingStopped()
    {
        _playerAnimation.StopRunning();
        _playerCollector.CheckCurrentNode(_currentPlatform);
    }

    private void SelectPlatform(RouteSelectorDirection direction)
    {
        _selectorDirection = direction;
        _currentPlatform.SetSelectorMaterials(direction);
    }

    private void RollDice()
    {
        _playerAnimation.RollDice();
        isDiceRolled = true;
        //maximumStep = Random.Range(1, 10);
    }
}
