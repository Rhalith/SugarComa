using Networking;
using UnityEngine;

namespace TempScripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject steamManagerObject;
        public SteamManager steamManager;
        public SteamServerManager serverManager;

        public GameObject playerPref;
        public GameObject playersParentObj;
    
        void Awake()
        {
            steamManagerObject = GameObject.Find("SteamManager");
            steamManager = steamManagerObject.GetComponent<SteamManager>();
            serverManager = GameObject.Find("ServerManager").GetComponent<SteamServerManager>();
        }

        void Start()
        {
            SpawnPlayers();
            SteamLobbyManager.Instance.inLobby.Clear();
        }

        void SpawnPlayers()
        {
            foreach (var friendObj in SteamLobbyManager.Instance.inLobby.Values)
            {
                GameObject obj = Instantiate(playerPref, playersParentObj.transform);
            }
        }
    }
}
