using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Platform _startplatform;
    [SerializeField] PathFinder _pathFinder;
    [SerializeField] MapCamera _mapCamera;
    [SerializeField] GameController _gameController;
    [SerializeField] GoalSelector _goalSelector;

    public PlayerInput currentPlayer;

    private GameObject _createdObject;


    public void CreatePlayer()
    {
        _createdObject = Instantiate(_playerPrefab);
        _createdObject.transform.position = new Vector3(0, 0, 0);
        ScriptKeeper sckeeper = _createdObject.GetComponent<ScriptKeeper>();
        SetPlayerMovement(_createdObject, sckeeper);
        SetPlayerCollector(_createdObject, sckeeper);
        SetGobletSelection(_createdObject, sckeeper);
        ChangeCurrentPlayer(currentPlayer, sckeeper._playerInput);
    }

    private void SetPlayerMovement(GameObject player, ScriptKeeper keeper)
    {
        keeper._playerMovement.SetCurrentPlatform(_startplatform);
        keeper._playerMovement.SetPathFinder(_pathFinder);
        keeper._playerMovement.SetMapCamera(_mapCamera);
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

    private void ChangeCurrentPlayer(PlayerInput currentPlayer, PlayerInput nextPlayer)
    {
        currentPlayer.isMyTurn = false;
        nextPlayer.isMyTurn = true;
    }
}

