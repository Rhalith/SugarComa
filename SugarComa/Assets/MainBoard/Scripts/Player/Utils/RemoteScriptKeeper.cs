using UnityEngine;
using Cinemachine;
using Assets.MainBoard.Scripts.Player.Remote;

namespace Assets.MainBoard.Scripts.Player.Utils
{
    public class RemoteScriptKeeper : MonoBehaviour
    {
        public int PlayerIndex;
        public RemotePlayerMovement _remotePlayerMovement;
        public CinemachineVirtualCamera _playerCamera;
    }
}