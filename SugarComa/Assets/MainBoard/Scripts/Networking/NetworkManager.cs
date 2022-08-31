using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.Handlers;
using Assets.MainBoard.Scripts.GameManaging;
using UnityEngine;
using System.Linq;

namespace Assets.MainBoard.Scripts.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        private static NetworkManager _instance;
        public static NetworkManager Instance => _instance;

        #region Fields
        private int _index;

        public int temp;

        public PlayerHandler playerHandler;
        #endregion

        #region Properties
        public int Index { get => _index; set => _index = value; }
        #endregion

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
            if (SteamManager.Instance.PlayerSteamId == SteamLobbyManager.currentLobby.Owner.Id)
            {
                CreatePlayers();
                playerHandler.mainPlayerStateContext.IsMyTurn = true;
            }
            SteamLobbyManager.Instance.inLobby.Clear();
        }

        private void OnApplicationQuit()
        {
            byte[] buffer = NetworkHelper.Serialize(new NetworkData(MessageType.Exit));
            SteamServerManager.Instance.SendingMessageToAll(buffer);
        }

        private void CreatePlayers()
        {
            if (SteamManager.Instance.PlayerSteamId == SteamLobbyManager.currentLobby.Owner.Id)
            {
                var steamIds = SteamLobbyManager.Instance.inLobby.Keys.ToArray();

                PlayerListNetworkData playerListData =
                       new(MessageType.CreatePlayers, NetworkHelper.SteamIdToByteArray(steamIds));

                bool result = SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(playerListData));
                if (result)
                {
                    PlayerTurnHandler.SpawnPlayers(steamIds);

                    var camera = playerHandler.GetCinemachineVirtualCamera(0);
                    camera.Priority = 2;
                }
            }
        }
    }
}