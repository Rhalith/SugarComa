using UnityEngine;
using Cinemachine;
using Assets.MainBoard.Scripts.Player.Remote;

namespace Assets.MainBoard.Scripts.Player.Utils
{
    public class RemoteScriptKeeper : MonoBehaviour
    {
        public int playerIndex;
        public RemotePlayerMovement remotePlayerMovement;
        public CinemachineVirtualCamera playerCamera;
    }
}