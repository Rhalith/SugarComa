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

    private bool isFirst = true;

    /// <summary>
    /// Creates player.
    /// </summary>
    public void CreatePlayer()
    {
        if (isFirst)
        {
            _playerList[0].SetActive(true);
            isFirst = false;
            return;
        }
        _createdObject = Instantiate(_playerPrefab);
        _createdObject.transform.position = new Vector3(0, 0, 0);
        _playerList.Add(_createdObject);
        ScriptKeeper sckeeper = _createdObject.GetComponent<ScriptKeeper>();
        SetPlayerMovement(sckeeper);
        SetPlayerCollector(sckeeper);
        SetGobletSelection(sckeeper);
        SetPlayerInput(sckeeper);
        SetPlayerSpec(sckeeper, _playerList.IndexOf(_createdObject)+1);
        ChangeCurrentPlayer();
    }


    /// <summary>
    /// Changes current player.
    /// </summary>
    public void ChangeCurrentPlayer() ///Knowing bug, e�er ilk oyuncu oynarken 3. oyuncuyu yarat�rsak kontrol 2. oyuncuya ge�iyor.
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
