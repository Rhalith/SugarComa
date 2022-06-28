using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Platform _startplatform;
    [SerializeField] PathFinder _pathFinder;
    [SerializeField] MapCamera _mapCamera;
    [SerializeField] GameController _gameController;
    [SerializeField] GoalSelector _goalSelector;
    [SerializeField] GameObject _currentUI;

    public PlayerInput currentPlayerInput;
    public PlayerInventory currentPlayerInventory;
    public PlayerCollector currentPlayerCollector;

    public TMP_Text currentplayerGold, currentplayerHealth, currentplayerGoblet;

    private GameObject _createdObject;


    public void CreatePlayer()
    {
        _createdObject = Instantiate(_playerPrefab);
        _createdObject.transform.position = new Vector3(0, 0, 0);
        ScriptKeeper sckeeper = _createdObject.GetComponent<ScriptKeeper>();
        SetPlayerMovement(_createdObject, sckeeper);
        SetPlayerCollector(_createdObject, sckeeper);
        SetGobletSelection(_createdObject, sckeeper);
        ChangePlayingPlayer(currentPlayerInput, sckeeper._playerInput);
        ChangeCurrentScripts(sckeeper._playerInput, sckeeper._playerCollector, sckeeper._playerInventory);
        ChangeCurrentUIElements(sckeeper.playerGold, sckeeper.playerHealth, sckeeper.playerGoblet);
        ChangeActiveUI(_currentUI, sckeeper._currentUI);
    }

    private void SetPlayerMovement(GameObject player, ScriptKeeper keeper)
    {
        keeper._playerMovement.SetCurrentPlatform(_startplatform);
        keeper._playerMovement.SetPathFinder(_pathFinder);
        keeper._playerMovement.SetMapCamera(_mapCamera);
        keeper._playerMovement.SetGameController(_gameController);
    }

    private void SetPlayerCollector(GameObject player, ScriptKeeper keeper)
    {
        keeper._playerCollector.SetGameController(_gameController);
    }

    private void SetGobletSelection(GameObject player, ScriptKeeper keeper)
    {
        keeper._gobletSelection.SetGameController(_gameController);
        keeper._gobletSelection.SetGoalSelector(_goalSelector);
    }

    private void ChangePlayingPlayer(PlayerInput currentInput, PlayerInput nextInput)
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
}

