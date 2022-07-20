using Networking;
using UnityEngine;

namespace TempScripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject steamManagerObject;
        public SteamManager steamManager;
        public SteamLobbyManager lobbyManager;
        public SteamServerManager serverManager;

        public GameObject playerPref;
        public GameObject playersParentObj;
    
        void Awake()
        {
            steamManagerObject = GameObject.Find("SteamManager");
            steamManager = steamManagerObject.GetComponent<SteamManager>();
            lobbyManager = steamManagerObject.GetComponent<SteamLobbyManager>();
            serverManager = GameObject.Find("ServerManager").GetComponent<SteamServerManager>();
        }

        void Start()
        {
            SpawnPlayers();
        }

        void SpawnPlayers()
        {
            foreach (var friendObj in lobbyManager.inLobby.Values)
            {
                GameObject obj = Instantiate(playerPref, playersParentObj.transform);
            }
        }
    }
}
