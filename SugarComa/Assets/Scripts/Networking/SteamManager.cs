using System;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Networking
{
    public class SteamManager : MonoBehaviour
    {
        public static SteamLobbyManager LobbyManager;
        public static SteamManager Instance;
        public uint appId;
    
        private bool isItHost = false;
        private bool applicationHasQuit;
        public string PlayerName { get; set; }
        public SteamId PlayerSteamId { get; set; }
        private string playerSteamIdString;
        public string PlayerSteamIdString { get => playerSteamIdString; }
    
        private bool connectedToSteam = false;
    
        public Lobby currentLobby;
        private Lobby hostedMultiplayerLobby;
    
        public UnityEvent OnSteamFailed;

        private void Awake()
        {
            if (Instance == null)
            {
                isItHost = true;
                DontDestroyOnLoad(gameObject);
                Instance = this;
                LobbyManager = GetComponent<SteamLobbyManager>();
                PlayerName = "";
                try
                {
                    // Create client 
                    SteamClient.Init(appId, true);

                    if (!SteamClient.IsValid)
                    {
                        Debug.Log("Steam client not valid");
                        throw new Exception();
                    }

                    PlayerName = SteamClient.Name;
                    PlayerSteamId = SteamClient.SteamId;
                    playerSteamIdString = PlayerSteamId.ToString();
                    connectedToSteam = true;
                    Debug.Log("Steam initialized: " + PlayerName);
                }
                catch (Exception e)
                {
                    connectedToSteam = false;
                    playerSteamIdString = "NoSteamId";
                    Debug.Log("Error connecting to Steam");
                    Debug.Log(e);
                }
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    
        public bool ConnectedToSteam()
        {
            return connectedToSteam;
        }

        void OnDisable()
        {
            if (isItHost)
            {
                GameCleanup();
            }
        }

        void OnDestroy()
        {
            if (isItHost)
            {
                GameCleanup();
            }
        }

        void OnApplicationQuit()
        {
            if (isItHost)
            {
                GameCleanup();
            }
        }
    
        // Place where you can update saves, etc. on sudden game quit as well
        private void GameCleanup()
        {
            if (!applicationHasQuit)
            {
                applicationHasQuit = true;
                leaveLobby();
                SteamClient.Shutdown();
            }
        }
    
        public void leaveLobby()
        {
            try
            {
                currentLobby.Leave();
            }
            catch
            {
                Debug.Log("Error leaving current lobby");
            }
            try
            {
                SteamNetworking.CloseP2PSessionWithUser(SteamClient.SteamId);
            }
            catch
            {
                Debug.Log("Error closing P2P session with opponent");
            }
        }
    }
}
