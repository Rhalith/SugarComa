using Assets.MainBoard.Scripts.Networking.LobbyNetworking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Networking;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using Steamworks;

public class LobbyMessageHandler : MonoBehaviour
{
    [SerializeField] private SteamLobbyManager steamLobbyManager;

    private void Awake()
    {
        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    private void OnDestroy()
    {
        SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
    }

    private void OnMessageReceived(SteamId steamId, byte[] buffer)
    {
        if (IsLobbyNetworData(steamId, buffer)) return;
    }

    #region Receivers
    private bool IsLobbyNetworData(SteamId steamId, byte[] buffer)
    {
        if (LobbyNetworkHelper.TryGetLobbyData(buffer, out LobbyData lobbyData))
        {
            switch (lobbyData.type)
            {
                case LobbyMessageType.Ready:
                    {
                        steamLobbyManager.UpdateLobbyFriend(steamId, lobbyData.type);
                    }
                    break;
                case LobbyMessageType.UnReady:
                    {
                        steamLobbyManager.UpdateLobbyFriend(steamId, lobbyData.type);
                    }
                    break;
                case LobbyMessageType.StartGame:
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    break;
                case LobbyMessageType.ReadyCheck:
                    {
                        if (steamLobbyManager.AmIReady)
                            SteamServerManager.Instance.SendingMessage(steamId, NetworkHelper.Serialize(new LobbyData(LobbyMessageType.Ready)));
                        else
                            SteamServerManager.Instance.SendingMessage(steamId, NetworkHelper.Serialize(new LobbyData(LobbyMessageType.UnReady)));
                    }
                    break;
                default:
                    throw new System.Exception();
            }
            return true;
        }

        return false;
    }
    #endregion

    #region Senders
    public void SendReadyToAll()
    {
        bool result = SteamServerManager.Instance
            .SendingMessageToAll(NetworkHelper.Serialize(new LobbyData(LobbyMessageType.Ready)));

        if (result)
        {
            steamLobbyManager.UpdateLobbyFriend(SteamManager.Instance.PlayerSteamId, LobbyMessageType.Ready);    
        }
    }

    public void SendUnreadyToAll()
    {
        bool result = SteamServerManager.Instance
            .SendingMessageToAll(NetworkHelper.Serialize(new LobbyData(LobbyMessageType.UnReady)));

        if (result)
        {
            steamLobbyManager.UpdateLobbyFriend(SteamManager.Instance.PlayerSteamId, LobbyMessageType.UnReady);
        }
    }

    public void StartGame()
    {
        if (steamLobbyManager.playerInfos.Any((playerInfo) => !playerInfo.Value.IsReady))
            return;

        bool result = SteamServerManager.Instance
            .SendingMessageToAll(NetworkHelper.Serialize(new LobbyData(LobbyMessageType.StartGame)));

        if (result)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    #endregion
}
