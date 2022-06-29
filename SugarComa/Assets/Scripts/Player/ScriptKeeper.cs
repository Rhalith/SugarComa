using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class ScriptKeeper : MonoBehaviour
{
    public PlayerMovement _playerMovement;
    public PlayerCollector _playerCollector;
    public GobletSelection _gobletSelection;
    public GoalSelector _goalSelector;
    public PlayerInput _playerInput;
    public PlayerInventory _playerInventory;
    public GameObject _currentUI;
    public TMP_Text playerGold, playerHealth, playerGoblet;
    public CinemachineVirtualCamera _playerCamera;
}
