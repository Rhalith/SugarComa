using Networking;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public GameObject playerPref;
    public GameObject remotePlayerPref;
    public GameObject playersParentObj;
    public Dictionary<SteamId, GameObject> playerList = new Dictionary<SteamId, GameObject>();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(this);
            return;
        }

        _instance = this;
    }

    void Start()
    {
        SpawnPlayers();
        SteamLobbyManager.Instance.inLobby.Clear();
    }

    void SpawnPlayers()
    {
        foreach (var id in SteamLobbyManager.Instance.inLobby.Keys)
        {

            if (id != SteamManager.Instance.PlayerSteamId)
            {
                GameObject obj = Instantiate(remotePlayerPref, playersParentObj.transform);
                playerList.Add(id, obj);
            }
            else
            {
                Instantiate(playerPref, playersParentObj.transform);
            }
        }
    }
}
