using TMPro;
using Cinemachine;
using UnityEngine;
using Assets.MainBoard.Scripts.UI;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Player.States;
using Assets.MainBoard.Scripts.Player.Movement;


namespace Assets.MainBoard.Scripts.Player.Utils
{
    public class ScriptKeeper : MonoBehaviour
    {
        public GameObject currentUI;
        public Transform playerTransform;
        public GoalSelector goalSelector;
        public PlayerCollector playerCollector;
        public PlayerInventory playerInventory;
        public GobletSelection gobletSelection;
        public CinemachineVirtualCamera playerCamera;
        public PlayerStateContext playerStateContext;
        public PlayerUIParentSetter playerUIParentSetter;
        public TMP_Text playerGold, playerHealth, playerGoblet;
    }
}