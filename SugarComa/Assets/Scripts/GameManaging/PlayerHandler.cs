using Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    #endregion

    #region HideInInspectors
    [HideInInspector] public PlayerInput currentPlayerInput;
    [HideInInspector] public PlayerInventory currentPlayerInventory;
    [HideInInspector] public PlayerCollector currentPlayerCollector;
    [HideInInspector] public TMP_Text currentplayerGold, currentplayerHealth, currentplayerGoblet;
    #endregion

    public int whichPlayer;
    private GameObject _createdObject;
    public Steamworks.SteamId[] _playerQueue;

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
        }
        else
        {
            _createdObject = Instantiate(_remotePlayerPrefab, playerParent.transform);
        }

        _createdObject.transform.position = new Vector3(0, 0, 0);
        ScriptKeeper sckeeper = _createdObject.GetComponent<ScriptKeeper>();
        SetPlayerMovement(sckeeper);
        SetPlayerCollector(sckeeper);
        SetGobletSelection(sckeeper);
        SetPlayerInput(sckeeper);
        SetPlayerSpec(sckeeper, ++playerCount);

        return _createdObject;
    }

    public void UpdateTurnQueue()
    {


        // Minigame'lere göre sıra belirlendiğinde buradan güncelleme yapılarak playerListData iletilebilir.

        PlayerListNetworkData playerListData =
               new PlayerListNetworkData(MessageType.UpdateQueue, NetworkHelper.SteamIdToByteArray(_playerList.Keys)));
        SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(playerListData));

        _playerQueue = _playerIdList.ToArray();
    }

    private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
    {
        if (NetworkHelper.TryGetPlayerListData(buffer, out PlayerListNetworkData playerListData))
        {
            if (playerListData.type == MessageType.UpdateQueue)
            {
                _playerQueue = NetworkHelper.ByteArrayToSteamId(playerListData.playerList);
            }
        }
    }

    /// <summary>
    /// Changes current player.
    /// </summary>
    public void ChangeCurrentPlayer() ///Knowing bug, eğer ilk oyuncu oynarken 3. oyuncuyu yaratırsak kontrol 2. oyuncuya geçiyor.
    {
        ScriptKeeper previouskeep = null;
        if (playerCount > 1)
        {
            whichPlayer++;

            // 3 tane liste var düzenlenmesi lazım... 1.GameObject List, 2.IdList, 3.IdListByQueue
            previouskeep = _playerList[_playerIdList.IndexOf(_playerQueue[whichPlayer - 1])].GetComponent<ScriptKeeper>();
            if (whichPlayer > _playerList.Count - 1) whichPlayer = 0;

            ScriptKeeper scKeeper = _playerList[_playerIdList.IndexOf(_playerQueue[whichPlayer])].GetComponent<ScriptKeeper>();
            ChangeCurrentSpecs(scKeeper, previouskeep);
        }
    }

    /// <summary>
    /// Changes current input, UI, Cam specifications.
    /// </summary>
    /// <param name="scriptKeeper"></param>
    /// <param name="previousKeep"></param>
    private void ChangeCurrentSpecs(ScriptKeeper scriptKeeper, ScriptKeeper previousKeep)
    {
        ChangePlayingInput(previousKeep._playerInput, scriptKeeper._playerInput);
        ChangeCurrentScripts(scriptKeeper._playerInput, scriptKeeper._playerCollector, scriptKeeper._playerInventory);
        ChangeUISpecs(scriptKeeper, previousKeep);
        ChangeCamSpecs(scriptKeeper, previousKeep);
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
    /// Changes Cam specifications.
    /// </summary>
    /// <param name="scriptKeeper"></param>
    /// <param name="previousKeep"></param>
    private void ChangeCamSpecs(ScriptKeeper scriptKeeper, ScriptKeeper previousKeep)
    {
        ChangeCamPriority(previousKeep, scriptKeeper);
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
        keeper._playerSpecSetter.SetParent(_playerSpecCanvas, index);
    }
    /// <summary>
    /// Activates next player's input and dice.
    /// </summary>
    /// <param name="currentInput"></param>
    /// <param name="nextInput"></param>
    private void ChangePlayingInput(PlayerInput currentInput, PlayerInput nextInput)
    {
        currentInput.isMyTurn = false;
        nextInput.isMyTurn = true;
        currentInput.Dice.SetActive(false);
        nextInput.Dice.SetActive(true);
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
    private void ChangeCamPriority(ScriptKeeper current, ScriptKeeper next)
    {
        current._playerCamera.Priority = 1;
        next._playerCamera.Priority = 2;
    }
}

