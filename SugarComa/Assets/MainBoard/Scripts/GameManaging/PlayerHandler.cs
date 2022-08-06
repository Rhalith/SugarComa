using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Utils.CamUtils;
using TMPro;
using UnityEngine;
using Cinemachine;
using System.Linq;

namespace Assets.MainBoard.Scripts.GameManaging
{
    [DefaultExecutionOrder(-101)]
    public class PlayerHandler : MonoBehaviour
    {
        private static PlayerHandler _instance;
        public static PlayerHandler Instance => _instance;

        #region SerializeFields
        [SerializeField] CameraAnimations _cameraAnimations;
        [SerializeField] GameObject _remotePlayerPrefab;
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] GameObject playerParent;
        [SerializeField] Platform _startplatform;
        [SerializeField] PathFinder _pathFinder;
        [SerializeField] MapCamera _mapCamera;
        [SerializeField] GameController _gameController;
        [SerializeField] GoalSelector _goalSelector;
        [SerializeField] Cinemachine.CinemachineBrain _cinemachineBrain;
        [SerializeField] GameObject _playerSpecCanvas;
        [SerializeField] MapCamera _mapCam;
        #endregion

        #region HideInInspectors
        [HideInInspector] public PlayerInput currentPlayerInput;
        [HideInInspector] public PlayerInventory currentPlayerInventory;
        [HideInInspector] public PlayerCollector currentPlayerCollector;
        [HideInInspector] public TMP_Text currentplayerGold, currentplayerHealth, currentplayerGoblet;
        #endregion

        public Steamworks.SteamId[] _playerQueue;
        private GameObject _createdObject;
        private int playerCount;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            _instance = this;
        }

        private void Start()
        {
            SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
            playerCount = 0;
        }

        private void OnDestroy()
        {
            SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
        }

        /// <summary>
        /// Creates player.
        /// </summary>
        public GameObject CreatePlayer(Steamworks.SteamId id)
        {
            if (SteamManager.Instance.PlayerSteamId == id)
            {
                _createdObject = Instantiate(_playerPrefab, playerParent.transform);
                _mapCam.mainCamera = _createdObject.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();

                _createdObject.transform.position = new Vector3(0, 0, 0);

                _mapCam.player = _createdObject.transform.GetChild(1).transform;


                ScriptKeeper sckeeper = _createdObject.GetComponent<ScriptKeeper>();
                SetPlayerInput(sckeeper);
                SetPlayerCollector(sckeeper);
                ChangeCurrentScripts(sckeeper._playerInput, sckeeper._playerCollector, sckeeper._playerInventory);
                ChangeCurrentUIElements(sckeeper.playerGold, sckeeper.playerHealth, sckeeper.playerGoblet);
                SetPlayerMovement(sckeeper);
                SetGobletSelection(sckeeper);
                SetPlayerSpec(sckeeper, ++playerCount);
            }   
            else
            {
                _createdObject = Instantiate(_remotePlayerPrefab, playerParent.transform);

                _createdObject.transform.position = new Vector3(0, 0, 0);
            }

            return _createdObject;
        }

        //TODO: System memory dll kullanılabilir performans'ı arttırmak için...
        public void UpdateTurnQueue(Steamworks.SteamId[] _playerList)
        {
            // Minigame'lere göre sıra belirlendiğinde buradan güncelleme yapılarak playerListData iletilebilir.
            _playerQueue = _playerList;

            PlayerListNetworkData playerListData =
                   new PlayerListNetworkData(MessageType.UpdateQueue, NetworkHelper.SteamIdToByteArray(_playerList));
            SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(playerListData));
        }

        private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
        {
            if (NetworkHelper.TryGetPlayerListData(buffer, out PlayerListNetworkData playerListData))
            {
                if (playerListData.type == MessageType.UpdateQueue)
                {
                    _playerQueue = NetworkHelper.ByteArrayToSteamId(playerListData.playerList);

                    ChangeCurrentPlayer(0);
                }
            }
            else if (NetworkHelper.TryGetTurnNetworkData(buffer, out TurnNetworkData turnNetworkData))
            {
                ChangeCurrentPlayer(turnNetworkData.index+1);
            }
        }

        /// <summary>
        /// Changes current player.
        /// </summary>
        public void ChangeCurrentPlayer(int index)
        {
            int prev = index - 1;
            if (index >= SteamLobbyManager.MemberCount)
            {
                index = 0;
            }
            if (index == 0)
                prev = SteamLobbyManager.MemberCount-1;

            if (NetworkManager.Instance.Index == index)
            {
                currentPlayerInput.isMyTurn = true;
                currentPlayerInput.Dice.SetActive(true);
            }

            CinemachineVirtualCamera current = NetworkManager.Instance.playerList.ElementAt(prev).Value.GetComponent<RemoteScriptKeeper>()._playerCamera;
            CinemachineVirtualCamera next = NetworkManager.Instance.playerList.ElementAt(index).Value.GetComponent<RemoteScriptKeeper>()._playerCamera;
            ChangeCamPriority(current, next);
        }

        /// <summary>
        /// Changes current input, UI, Cam specifications.
        /// </summary>
        /// <param name="scriptKeeper"></param>
        /// <param name="previousKeep"></param>
        private void ChangeCurrentSpecs(ScriptKeeper scriptKeeper, ScriptKeeper previousKeep)
        {
            ChangeUISpecs(scriptKeeper, previousKeep);
        }

        /// <summary>
        /// Changes UI specifications.
        /// </summary>
        /// <param name="scriptKeeper"></param>
        /// <param name="previousKeep"></param>
        private void ChangeUISpecs(ScriptKeeper scriptKeeper, ScriptKeeper previousKeep)
        {
            ChangeCurrentUIElements(scriptKeeper.playerGold, scriptKeeper.playerHealth, scriptKeeper.playerGoblet);
        }

        /// <summary>
        /// Sets player movement variables by using ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        private void SetPlayerMovement(ScriptKeeper keeper)
        {
            keeper._playerMovement.MapCamera = _mapCamera;
            keeper._playerMovement.PathFinder = _pathFinder;
            keeper._playerMovement.CurrentPlatform = _startplatform;
            keeper._playerMovement.GameController = _gameController;
        }

        /// <summary>
        /// Sets player input variables by using ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        private void SetPlayerInput(ScriptKeeper keeper)
        {
            keeper._playerInput.CineMachineBrain = _cinemachineBrain;
        }

        /// <summary>
        /// Sets player collector variables by using ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        private void SetPlayerCollector(ScriptKeeper keeper)
        {
            keeper._playerCollector.GameController = _gameController;
        }

        /// <summary>
        /// Sets goblet selection variables by using ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        private void SetGobletSelection(ScriptKeeper keeper)
        {
            keeper._goalSelector = _goalSelector;
            keeper._gobletSelection.GameController = _gameController;
            keeper._gobletSelection.GoalSelector = _goalSelector;
            keeper._gobletSelection.PathFinder = _pathFinder;
            keeper._playerAnimation.GoalSelector = _goalSelector;
        }

        /// <summary>
        /// Makes player UI child of _playerSpecCanvas for automatic line up.
        /// </summary>
        /// <param name="keeper"></param>
        /// <param name="index"></param>
        private void SetPlayerSpec(ScriptKeeper keeper, int index)
        {
            keeper._playerUIParentSetter.SetParent(_playerSpecCanvas, index);
        }

        /// <summary>
        /// Changes current scripts variables for check or use them.
        /// </summary>
        /// <param name="nextInput"></param>
        /// <param name="nextCollector"></param>
        /// <param name="nextInventory"></param>
        private void ChangeCurrentScripts(PlayerInput nextInput, PlayerCollector nextCollector, PlayerInventory nextInventory)
        {
            currentPlayerInput = nextInput;
            currentPlayerCollector = nextCollector;
            currentPlayerInventory = nextInventory;
        }

        /// <summary>
        /// Changes current UI variables for check or use them.
        /// </summary>
        /// <param name="playerGold"></param>
        /// <param name="playerHealth"></param>
        /// <param name="playerGoblet"></param>
        private void ChangeCurrentUIElements(TMP_Text playerGold, TMP_Text playerHealth, TMP_Text playerGoblet)
        {
            currentplayerGold = playerGold;
            currentplayerHealth = playerHealth;
            currentplayerGoblet = playerGoblet;
        }

        /// <summary>
        /// Makes current player's cam priority higher.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="next"></param>
        private void ChangeCamPriority(CinemachineVirtualCamera current, CinemachineVirtualCamera next)
        {
            current.Priority = 1;
            next.Priority = 2;
        }
    }
}