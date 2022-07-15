using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-101)]
public class PlayerHandler : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] CameraAnimations _cameraAnimations;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Platform _startplatform;
    [SerializeField] PathFinder _pathFinder;
    [SerializeField] MapCamera _mapCamera;
    [SerializeField] GameController _gameController;
    [SerializeField] GoalSelector _goalSelector;
    [SerializeField] List<GameObject> _playerList;
    [SerializeField] Cinemachine.CinemachineBrain _cinemachineBrain;
    [SerializeField] GameObject _playerSpecCanvas;
    #endregion

    #region HideInInspectors
    [HideInInspector] public PlayerInput currentPlayerInput;
    [HideInInspector] public PlayerInventory currentPlayerInventory;
    [HideInInspector] public PlayerCollector currentPlayerCollector;
    [HideInInspector] public TMP_Text currentplayerGold, currentplayerHealth, currentplayerGoblet;
    #endregion
    private GameObject _createdObject;
    public int whichPlayer;

    public void CreatePlayer()
    {
        _createdObject = Instantiate(_playerPrefab);
        _createdObject.transform.position = new Vector3(0, 0, 0);
        _playerList.Add(_createdObject);
        ScriptKeeper sckeeper = _createdObject.GetComponent<ScriptKeeper>();
        SetPlayerMovement(sckeeper);
        SetPlayerCollector(sckeeper);
        SetGobletSelection(sckeeper);
        SetPlayerInput(sckeeper);
        setPlayerSpec(sckeeper, _playerList.IndexOf(_createdObject)+1);
        ChangeCurrentPlayer();
    }


    /// <summary>
    /// Knowing bug, e�er ilk oyuncu oynarken 3. oyuncuyu yarat�rsak kontrol 2. oyuncuya ge�iyor.
    /// </summary>
    public void ChangeCurrentPlayer()
    {
        ScriptKeeper previouskeep = null;
        if (_playerList.Count > 0)
        {
            whichPlayer++;
            previouskeep = _playerList[whichPlayer - 1].GetComponent<ScriptKeeper>();
            if (whichPlayer > _playerList.Count - 1) whichPlayer = 0;
        }
        ScriptKeeper scKeeper = _playerList[whichPlayer].GetComponent<ScriptKeeper>();
        ChangeCurrentSpecs(scKeeper, previouskeep);
    }

    private void ChangeCurrentSpecs(ScriptKeeper scriptKeeper, ScriptKeeper previousKeep)
    {
        ChangePlayingInput(previousKeep._playerInput, scriptKeeper._playerInput);
        ChangeCurrentScripts(scriptKeeper._playerInput, scriptKeeper._playerCollector, scriptKeeper._playerInventory);
        ChangeCurrentUIElements(scriptKeeper.playerGold, scriptKeeper.playerHealth, scriptKeeper.playerGoblet);
        ChangeActiveUI(previousKeep._currentUI, scriptKeeper._currentUI);
        ChangeCameraAnimations(scriptKeeper);
        ChangeCamPriority(previousKeep, scriptKeeper);
    }
    private void SetPlayerMovement(ScriptKeeper keeper)
    {
        keeper._playerMovement.MapCamera = _mapCamera;
        keeper._playerMovement.PathFinder = _pathFinder;
        keeper._playerMovement.CurrentPlatform = _startplatform;
        keeper._playerMovement.GameController = _gameController;
    }

    private void SetPlayerInput(ScriptKeeper keeper)
    {
        keeper._playerInput.CineMachineBrain = _cinemachineBrain;
    }

    private void SetPlayerCollector(ScriptKeeper keeper)
    {
        keeper._playerCollector.GameController = _gameController;
    }

    private void SetGobletSelection(ScriptKeeper keeper)
    {
        keeper._goalSelector = _goalSelector;
        keeper._gobletSelection.GameController = _gameController;
        keeper._gobletSelection.GoalSelector = _goalSelector;
        keeper._gobletSelection.PathFinder = _pathFinder;
        keeper._playerAnimation.GoalSelector = _goalSelector;
    }

    private void setPlayerSpec(ScriptKeeper keeper, int index)
    {
        keeper._playerSpecSetter.SetParent(_playerSpecCanvas, index);
    }
    private void ChangePlayingInput(PlayerInput currentInput, PlayerInput nextInput)
    {
        currentInput.isMyTurn = false;
        nextInput.isMyTurn = true;
    }

    private void ChangeCurrentScripts(PlayerInput nextInput, PlayerCollector nextCollector, PlayerInventory nextInventory)
    {
        currentPlayerInput = nextInput;
        currentPlayerCollector = nextCollector;
        currentPlayerInventory = nextInventory;
    }

    private void ChangeCurrentUIElements(TMP_Text playerGold, TMP_Text playerHealth, TMP_Text playerGoblet)
    {
        currentplayerGold = playerGold;
        currentplayerHealth = playerHealth;
        currentplayerGoblet = playerGoblet;
    }
    private void ChangeActiveUI(GameObject currentUI, GameObject nextUI)
    {
        currentUI.SetActive(false);
        nextUI.SetActive(true);
    }
    private void ChangeCamPriority(ScriptKeeper current, ScriptKeeper next)
    {
        current._playerCamera.Priority = 1;
        next._playerCamera.Priority = 2;
    }

    private void ChangeCameraAnimations(ScriptKeeper next)
    {
        _cameraAnimations.SetGobletSelection(next._gobletSelection);
    }
}

