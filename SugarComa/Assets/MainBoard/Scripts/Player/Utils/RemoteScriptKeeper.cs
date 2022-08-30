using Assets.MainBoard.Scripts.Player.Remote;
using Assets.MainBoard.Scripts.UI;
using UnityEngine;
using Cinemachine;
using TMPro;

namespace Assets.MainBoard.Scripts.Player.Utils
{
    public class RemoteScriptKeeper : MonoBehaviour
    {
        public int playerIndex;
        public RemotePlayerMovement remotePlayerMovement;
        public CinemachineVirtualCamera playerCamera;
        public RemotePlayerCollector playerCollector;
        public PlayerUIParentSetter playerUIParentSetter;
        public TMP_Text playerGold, playerHealth, playerGoblet;
    }
}