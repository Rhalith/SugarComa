using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
	public bool isMyTurn;
	[HideInInspector] public bool nextSelectionPressed;
	[HideInInspector] public bool nextSelectionStepPressed;
	[HideInInspector] public bool nextGoalPressed;
	[HideInInspector] public bool nextGoalStepPressed;
	[HideInInspector] public bool moveToBackStepPressed;
	[HideInInspector] public bool selectLeftPressed;
	[HideInInspector] public bool selectRightPressed;
	[HideInInspector] public bool applySelectPressed;
	[HideInInspector] public bool openInventory;
	[HideInInspector] public bool closeUI;
	[HideInInspector] public bool openMap;
	[HideInInspector] public bool useMouseItem;

	[SerializeField] Cinemachine.CinemachineBrain cinemachineBrain;

	private bool _readyToClear; // used to keep input in sync
	private bool _clearNextTick = false;

	/// <summary>
	/// if an animation or action will play to all players.
	/// </summary>
	public static bool canPlayersAct = true;

	public Cinemachine.CinemachineBrain CineMachineBrain { set => cinemachineBrain = value; }
	void Update()
	{
		_clearNextTick = false;

		ClearInputs();

		if (GameManager.IsGameOver) return;

		if(isMyTurn && canPlayersAct && !cinemachineBrain.IsBlending) ProcessInputs();
	}

	void FixedUpdate()
	{
		_readyToClear = true;

		// make sure inputs cleared.
		// only if fixed update being called more than update.
		if (_clearNextTick)
		{
			ClearInputs();
			_clearNextTick = false;
		}
		_clearNextTick = true;
	}

	void ClearInputs()
	{
		//If we're not ready to clear input, return
		if (!_readyToClear) return;

		//Reset all inputs
		nextSelectionPressed = false;
		nextSelectionStepPressed = false;
		nextGoalPressed = false;
		nextGoalStepPressed = false;
		moveToBackStepPressed = false;
		selectLeftPressed = false;
		selectRightPressed = false;
		applySelectPressed = false;
		openInventory = false;
		closeUI = false;
		openMap = false;

		_readyToClear = false;
	}

	void ProcessInputs()
	{
		nextSelectionPressed = nextSelectionPressed || Input.GetKeyDown(KeyCode.X);
		nextSelectionStepPressed = nextSelectionStepPressed || Input.GetKeyDown(KeyCode.Space);
		nextGoalPressed = nextGoalPressed || Input.GetKeyDown(KeyCode.C);
		nextGoalStepPressed = nextGoalStepPressed || Input.GetKeyDown(KeyCode.V);
		moveToBackStepPressed = moveToBackStepPressed || Input.GetKeyDown(KeyCode.B);
		selectLeftPressed = selectLeftPressed || Input.GetKeyDown(KeyCode.A);
		selectRightPressed = selectRightPressed || Input.GetKeyDown(KeyCode.D);
		applySelectPressed = applySelectPressed || Input.GetKeyDown(KeyCode.Return);
		openInventory = openInventory || Input.GetKeyDown(KeyCode.I);
		closeUI = closeUI || Input.GetKeyDown(KeyCode.Escape);
		openMap = openMap || Input.GetKeyDown(KeyCode.M);
		useMouseItem = (useMouseItem || Input.GetMouseButtonDown(0)) && ItemPool._isItemUsing;
	}
}
