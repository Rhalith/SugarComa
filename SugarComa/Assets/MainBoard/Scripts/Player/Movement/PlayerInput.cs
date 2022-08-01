using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Movement
{
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
        [SerializeField] GameObject _dice;

        private bool _readyToClear; // used to keep input in sync
        private bool _clearNextTick = false;

        /// <summary>
        /// if an animation or action will play to all players.
        /// </summary>
        public static bool canPlayersAct = true;

        public Cinemachine.CinemachineBrain CineMachineBrain { set => cinemachineBrain = value; }
        public GameObject Dice { get => _dice; }
        void Update()
        {
            _clearNextTick = false;

            ClearInputs();

            // Last Check: Checks if it is this client's own object
            // We can create 2 object, one for client's own player object, one for remote player object for other clients movements.
            // After that we won't need that check any more
            if (GameManager.IsGameOver) return;
            if (isMyTurn && canPlayersAct && !cinemachineBrain.IsBlending
                && PlayerHandler.Instance._playerIdList[0] == SteamManager.Instance.PlayerSteamId)
            {
                ProcessInputs();
            }
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
}