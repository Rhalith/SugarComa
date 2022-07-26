using Networking;
using Steamworks;
using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    public static NetworkManager Instance => _instance;

    public const int MaxPlayerCount = 8;

    // Ayrý bir yerde tutulabilir.
    public Dictionary<SteamId, GameObject> playerList = new Dictionary<SteamId, GameObject>();

    public PlayerHandler playerHandler;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(this);
            return;
        }

        _instance = this;
        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(SteamId steamid, byte[] buffer)
    {
        if (NetworkHelper.TryGetNetworkData(buffer, out NetworkData data) && data.type == MessageType.Exit)
        {
            if (playerList.TryGetValue(steamid, out GameObject gameObject))
            {
                Destroy(gameObject);
                playerList.Remove(steamid);
                SteamLobbyManager.Instance.playerInfos.Remove(steamid);
            }
        }
    }

    void Start()
    {
        SpawnPlayers();
        SteamLobbyManager.Instance.inLobby.Clear();
        playerHandler.UpdateTurnQueue();
    }

    private void OnApplicationQuit()
    {
        byte[] buffer = NetworkHelper.Serialize(new NetworkData(MessageType.Exit));
        SteamServerManager.Instance.SendingMessageToAll(buffer);
    }

    void SpawnPlayers()
    {
        foreach (var id in SteamLobbyManager.Instance.inLobby.Keys)
        {

            if (id != SteamManager.Instance.PlayerSteamId)
            {
                playerList.Add(id, playerHandler.CreatePlayer(id));
            }
            else
            {
                playerHandler.CreatePlayer(id);
            }
        }
    }
}
