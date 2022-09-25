using Assets.MainBoard.Scripts.Networking;
using Assets.MiniGames.FallingStars.Scripts.Networking.Utils;
using Assets.MiniGames.FallingStars.Scripts.Player.Remote;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Networking
{
    public class MessageHandler : MonoBehaviour
    {
        private void Awake()
        {
            SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
        }

        private void OnDestroy()
        {
            SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
        }
        private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
        {
            if (!NetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
            {
                return;
            }

            switch (networkData.type)
            {
                case MessageType.Move:
                    {
                        GameObject remotePlayerObj = PlayerHandler.Instance.RemotePlayerList[steamid];
                        remotePlayerObj.GetComponent<RemotePlayerMovement>().MovementDir = networkData.moveDir;
                    }
                    break;
                case MessageType.Rotate:
                    {
                        GameObject remotePlayerObj = PlayerHandler.Instance.RemotePlayerList[steamid];
                        remotePlayerObj.GetComponent<RemotePlayerMovement>().RotationDir = networkData.rotationDir;
                    }
                    break;
                default:
                    throw new System.Exception();
            }
        }
    }
}