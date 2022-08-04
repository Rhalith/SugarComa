using Steamworks;
using UnityEngine;
using System.Collections.Generic;
using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Networking.Utils;
using System.Linq;

namespace Assets.MainBoard.Scripts.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        private static NetworkManager _instance;
        public static NetworkManager Instance => _instance;

        private int _index;
        public int Index => _index;

        public int temp;
        // Ayrı bir yerde tutulabilir.
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

            if(SteamManager.Instance.PlayerSteamId == SteamLobbyManager.currentLobby.Owner.Id)
            {
                playerHandler.UpdateTurnQueue(playerList.Keys.ToArray());
                playerHandler.currentPlayerInput.isMyTurn = true;
                playerHandler.currentPlayerInput.Dice.SetActive(true);
            }
        }

        private void OnApplicationQuit()
        {
            byte[] buffer = NetworkHelper.Serialize(new NetworkData(MessageType.Exit));
            SteamServerManager.Instance.SendingMessageToAll(buffer);
        }

        void SpawnPlayers()
        {
            int i = 0;
            foreach (var id in SteamLobbyManager.Instance.inLobby.Keys)
            {
                 playerList.Add(id, playerHandler.CreatePlayer(id));
                if (id == SteamManager.Instance.PlayerSteamId) _index = i;
                i++;
            }

            temp = _index;
        }
    }
}