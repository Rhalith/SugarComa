using UnityEngine;
using TMPro;
using Cinemachine;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.UI;
using Assets.MainBoard.Scripts.Player.States;

namespace Assets.MainBoard.Scripts.Player.Utils
{
    public class ScriptKeeper : MonoBehaviour
    {
        public PlayerMovement _playerMovement;
        public PlayerCollector _playerCollector;
        public GobletSelection _gobletSelection;
        public GoalSelector _goalSelector;
        public PlayerStateContext _playerStateContext;
        public PlayerInventory _playerInventory;
        public GameObject _currentUI;
        public TMP_Text playerGold, playerHealth, playerGoblet;
        public CinemachineVirtualCamera _playerCamera;
        public Transform playerTransform;
        public PlayerUIParentSetter _playerUIParentSetter;
    }
}