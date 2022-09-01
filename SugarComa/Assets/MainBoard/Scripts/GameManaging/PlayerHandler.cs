using TMPro;
using Steamworks;
using Cinemachine;
using UnityEngine;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Player.States;
using Assets.MainBoard.Scripts.Player.Remote;
using Assets.MainBoard.Scripts.Utils.CamUtils;
using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Player.Handlers;
using Assets.MainBoard.Scripts.Networking.Utils;

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

        // TODO: CinemachineBrain kullanılmıyo???
        [SerializeField] Cinemachine.CinemachineBrain _cinemachineBrain;
        [SerializeField] GameObject _playerSpecCanvas;
        #endregion

        #region HideInInspectors
        [HideInInspector] public PlayerStateContext mainPlayerStateContext;
        [HideInInspector] public PlayerInventory mainPlayerInventory;
        [HideInInspector] public PlayerCollector mainPlayerCollector;
        [HideInInspector] public TMP_Text mainplayerGold, mainplayerHealth, mainplayerGoblet;
        #endregion

        #region Properties
        public MapCamera MapCamera => _mapCamera;
        #endregion

        #region Public Fields
        public Steamworks.SteamId[] _playerQueue;
        #endregion

        #region Private Fields
        private GameObject _createdObject;
        private int playerCount;
        #endregion

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

        /// <summary>
        /// Creates player.
        /// </summary>
        public GameObject CreatePlayer(SteamId id)
        {
            if (SteamManager.Instance.PlayerSteamId == id)
            {
                _createdObject = Instantiate(_playerPrefab, playerParent.transform);
                _createdObject.transform.position = new Vector3(0, 0, 0);

                ScriptKeeper scKeeper = _createdObject.GetComponent<ScriptKeeper>();

                SetScripts(scKeeper);
                UpdateMapCam(scKeeper);
                UpdateStateContext();

                // TODO: Tracker zaten tanımlandığı için eventleri initialize edebiliriz running'de... (Silinebilir mi?)
                mainPlayerStateContext.Running.InitializePathTracker();

                SetUIElements(scKeeper.playerGold, scKeeper.playerHealth, scKeeper.playerGoblet);
                SetGobletSelection(scKeeper);
                SetPlayerSpec(scKeeper, null, ++playerCount);
            }
            else
            {
                _createdObject = Instantiate(_remotePlayerPrefab, playerParent.transform);

                GameObject stone = _createdObject.transform.GetChild(1).gameObject;

                _createdObject.transform.position = new Vector3(0, 0, 0);
                stone.transform.position = new Vector3(0, 0.25f, 0);

                RemoteScriptKeeper RemoteScKeeper = _createdObject.GetComponent<RemoteScriptKeeper>();
                stone.GetComponent<RemotePlayerCollector>().GameController = _gameController;

                SetPlayerSpec(null, RemoteScKeeper, ++playerCount);
            }
            return _createdObject;
        }


        public void UpdateTurnQueue(SteamId[] _playerList)
        {
            // TODO:  Minigame'lere göre sıra belirlendiğinde buradan güncelleme yapılarak playerListData iletilebilir.
            _playerQueue = _playerList;

            PlayerListNetworkData playerListData =
                   new PlayerListNetworkData(MessageType.UpdatePlayers, NetworkHelper.SteamIdToByteArray(_playerList));
            bool result = SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(playerListData));
        }

        private void OnMessageReceived(SteamId steamid, byte[] buffer)
        {
            if (NetworkHelper.TryGetPlayerListData(buffer, out PlayerListNetworkData playerListData))
            {
                if (playerListData.type == MessageType.UpdatePlayers)
                {
                    _playerQueue = NetworkHelper.ByteArrayToSteamId(playerListData.playerList);

                    ChangeCurrentPlayer(0);
                }
            }
            else if (NetworkHelper.TryGetTurnNetworkData(buffer, out TurnNetworkData turnNetworkData))
            {
                PlayerTurnHandler.NextPlayer();
                ChangeCurrentPlayer(PlayerTurnHandler.Index);
            }
        }

        /// <summary>
        /// Changes current player.
        /// </summary>
        public void ChangeCurrentPlayer(int nextIndex)
        {
            int currentIndex = nextIndex - 1;
            if (nextIndex >= SteamLobbyManager.MemberCount) nextIndex = 0;
            if (nextIndex == 0) currentIndex = SteamLobbyManager.MemberCount - 1;

            if (NetworkManager.Instance.Index == nextIndex)
                mainPlayerStateContext.IsMyTurn = true;

            CinemachineVirtualCamera current = GetCinemachineVirtualCamera(currentIndex);
            CinemachineVirtualCamera next = GetCinemachineVirtualCamera(nextIndex);
            
            ChangeCamPriority(current, next);
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

        #region Setters
        /// <summary>
        /// Changes current scripts variables for check or use them.
        /// </summary>
        /// <param name="nextInput"></param>
        /// <param name="nextCollector"></param>
        /// <param name="nextInventory"></param>
        private void SetScripts(ScriptKeeper scKeeper)
        {
            mainPlayerStateContext = scKeeper.playerStateContext;
            mainPlayerInventory = scKeeper.playerInventory;
            mainPlayerCollector = scKeeper.playerCollector;
            mainPlayerCollector.GameController = _gameController;
        }

        // Mapcam atamaları
        private void UpdateMapCam(ScriptKeeper scKeeper)
        {
            _mapCamera.mainCamera = scKeeper.playerCamera;
            _mapCamera.player = scKeeper.playerTransform;
        }

        private void UpdateStateContext()
        {
            mainPlayerStateContext.Idle.MapCamera = _mapCamera;
            mainPlayerStateContext.Running.CurrentPlatform = _startplatform;
            mainPlayerStateContext.Running.PathFinder = _pathFinder;
            mainPlayerStateContext.Death.PathFinder = _pathFinder;
            mainPlayerStateContext.Land.GoalSelector = _goalSelector;
            mainPlayerStateContext.PlayerHandler = this;
        }

        /// <summary>
        /// Sets goblet selection variables by using ScriptKeeper.
        /// </summary>
        /// <param name="keeper"></param>
        private void SetGobletSelection(ScriptKeeper keeper)
        {
            keeper.goalSelector = _goalSelector;
            keeper.gobletSelection.GameController = _gameController;
            keeper.gobletSelection.GoalSelector = _goalSelector;
            keeper.gobletSelection.PathFinder = _pathFinder;
        }
        /// <summary>
        /// Changes current UI variables for check or use them.
        /// </summary>
        /// <param name="playerGold"></param>
        /// <param name="playerHealth"></param>
        /// <param name="playerGoblet"></param>
        private void SetUIElements(TMP_Text playerGold, TMP_Text playerHealth, TMP_Text playerGoblet)
        {
            mainplayerGold = playerGold;
            mainplayerHealth = playerHealth;
            mainplayerGoblet = playerGoblet;
        }

        /// <summary>
        /// Makes player UI child of _playerSpecCanvas for automatic line up.
        /// </summary>
        /// <param name="keeper"></param>
        /// <param name="index"></param>
        private void SetPlayerSpec(ScriptKeeper keeper, RemoteScriptKeeper remoteKeeper, int index)
        {
            if( remoteKeeper == null)
                keeper.playerUIParentSetter.SetParent(_playerSpecCanvas, index);
            else
                remoteKeeper.playerUIParentSetter.SetParent(_playerSpecCanvas, index);
        }

        public CinemachineVirtualCamera GetCinemachineVirtualCamera(int index)
        {
            var player = PlayerTurnHandler.Players[index];

            if (NetworkManager.Instance.Index == index)
                return player.GetComponent<ScriptKeeper>().playerCamera;

            return player.GetComponent<RemoteScriptKeeper>().playerCamera;
        }
        #endregion
    }
}