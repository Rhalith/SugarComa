using Steamworks;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Steamworks.Data;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class SteamLobbyManager : MonoBehaviour
    {
        private static SteamLobbyManager _instance;
        public static SteamLobbyManager Instance => _instance;

        public Transform content;
        public UnityEngine.UI.Image panelImage;
        public static SteamManager steamManager;
        public static Lobby currentLobby;
        public static bool UserInLobby;

        #region Events

        public UnityEvent OnLobbyCreated;
        public UnityEvent OnLobbyJoined;
        public UnityEvent OnLobbyLeave;
        #endregion

        public GameObject InLobbyFriend;
        public int MemberCount => currentLobby.MemberCount;
        public Dictionary<SteamId, PlayerInfo> playerInfos = new Dictionary<SteamId, PlayerInfo>();
        public Dictionary<SteamId, GameObject> inLobby = new Dictionary<SteamId, GameObject>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
            steamManager = GetComponent<SteamManager>();

            SceneManager.activeSceneChanged += SceneManager_ActiveSceneChanged;
            SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
        }

        private void Start()
        {
            #region Lobby Events
            SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallBack;
            SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallBack;
            SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoinedCallBack;
            SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberDisconnectedCallBack;
            SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberDisconnectedCallBack;
            SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequestCallBack;
            #endregion
        }

        private void SceneManager_ActiveSceneChanged(Scene arg0, Scene arg1)
        {
            SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;

            SceneManager.activeSceneChanged -= SceneManager_ActiveSceneChanged;

            #region Lobby Events
            SteamMatchmaking.OnLobbyCreated -= OnLobbyCreatedCallBack;
            SteamMatchmaking.OnLobbyEntered -= OnLobbyEnteredCallBack;
            SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoinedCallBack;
            SteamMatchmaking.OnLobbyMemberDisconnected -= OnLobbyMemberDisconnectedCallBack;
            SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberDisconnectedCallBack;
            SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequestCallBack;
            #endregion
        }

        // For now i'll use a button instead of check if everybody ready.
        public void StartGame()
        {
            if (Instance.playerInfos.Any((playerInfo) => !playerInfo.Value.IsReady))
                return;

            bool result = SteamServerManager.Instance
                .SendingMessageToAll(NetworkHelper.Serialize(new NetworkData(MessageType.StartGame)));

            if (result)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        private void OnGameStarted()
        {
        }

        void Update()
        {
            SteamClient.RunCallbacks();
        }

        private async void OnLobbyMemberJoinedCallBack(Lobby lobby, Friend friend)
        {
            Debug.Log($"{friend.Name} joined the lobby");
            // Bu kısım ayrı bir fonksiyona dönüştürülebilir...

            if (await CreatePlayer(friend.Id, friend.Name))
            {
                AcceptP2P(friend.Id);
            }
        }

        public void SendReadyToAll()
        {
            bool result = SteamServerManager.Instance
                .SendingMessageToAll(NetworkHelper.Serialize(new NetworkData(MessageType.Ready)));
            if (result)
            {
                playerInfos[SteamManager.Instance.PlayerSteamId].IsReady = true;
                panelImage.color = UnityEngine.Color.green;
            }
        }
        
        // Bu değişiklikler için observer pattern kullanabilir miyiz?

        public void SendUnreadyToAll()
        {
            bool result = SteamServerManager.Instance
                .SendingMessageToAll(NetworkHelper.Serialize(new NetworkData(MessageType.UnReady)));
            if (result)
            {
                playerInfos[SteamManager.Instance.PlayerSteamId].IsReady = false;
                panelImage.color = UnityEngine.Color.red;
            }
        }
        
        private void OnMessageReceived(SteamId steamid, byte[] buffer)
        {
            if (!NetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
            {
                return;
            }

            switch (networkData.type)
            {
                case MessageType.Ready:
                    {
                        playerInfos[steamid].IsReady = true;
                        // inLobby[steamid].active = green;
                        // panelImage.color = UnityEngine.Color.green;
                    }
                    break;
                case MessageType.UnReady:
                    {
                        playerInfos[steamid].IsReady = false;
                        // inLobby[steamid].active = green;
                        //panelImage.color = UnityEngine.Color.red;
                    }
                    break;
                case MessageType.StartGame:
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    break;
                default:
                    throw new System.Exception();
            }
        }

        void OnLobbyMemberDisconnectedCallBack(Lobby lobby, Friend friend)
        {
            Debug.Log($"{friend.Name} left the lobby");
            Debug.Log($"New lobby owner is {currentLobby.Owner}");
            if (inLobby.TryGetValue(friend.Id, out GameObject gameObject))
            {
                Destroy(gameObject);
                inLobby.Remove(friend.Id);
                playerInfos.Remove(friend.Id);
            }
        }

        async void OnGameLobbyJoinRequestCallBack(Lobby joinedLobby, SteamId id)
        {
            RoomEnter joinedLobbySuccess = await joinedLobby.Join();
            if (joinedLobbySuccess != RoomEnter.Success)
            {
                Debug.Log("failed to join lobby : " + joinedLobbySuccess);
            }
        }
    
        private void AcceptP2P(SteamId friendId)
        {
            try
            {
                SteamNetworking.AcceptP2PSessionWithUser(friendId);
            }
            catch
            {
                Debug.Log("Unable to accept P2P Session with user");
            }
        }

        void OnLobbyCreatedCallBack(Result result, Lobby lobby)
        {
            if (result != Result.OK)
            {
                Debug.Log("lobby creation result not ok : " + result);
            }
            else
            {
                OnLobbyCreated.Invoke();
                Debug.Log("lobby creation result ok");
            }
        }

        async void OnLobbyEnteredCallBack(Lobby lobby)
        {
            currentLobby = lobby;

            Debug.Log("Client joined the lobby");
            UserInLobby = true;
            foreach (var user in inLobby.Values)
            {
                Destroy(user);
            }
            inLobby.Clear();

            await CreatePlayer(steamManager.PlayerSteamId, SteamClient.Name);

            OnLobbyJoined.Invoke();

            int count = currentLobby.MemberCount - 1;
            if (count <= 0) return;

            List<Task<bool>> tasks = new List<Task<bool>>(count);
            foreach (var friend in currentLobby.Members)
            {
                if (friend.Id != SteamClient.SteamId)
                {
                    tasks.Add(CreatePlayer(friend.Id, friend.Name));
                    AcceptP2P(friend.Id);
                }
            }
            Task.WaitAll(tasks.ToArray());
        }
    
        public async void CreateLobbyAsync()
        {
            bool result = await CreateLobby();
            if (!result)
            {
                //Invoke an error message.
            }
        }

        public static async Task<bool> CreateLobby()
        {
            try
            {
                var createLobbyOutput = await SteamMatchmaking.CreateLobbyAsync();
                if (!createLobbyOutput.HasValue)
                {
                    Debug.Log("Lobby created but not correctly instantiated.");
                    return false;
                }
                currentLobby = createLobbyOutput.Value;

                currentLobby.SetPublic();
                //currentLobby.SetPrivate();
                currentLobby.SetJoinable(true);

                return true;
            }
            catch(System.Exception exception)
            {
                Debug.Log("Failed to create multiplayer lobby : " + exception);
                return false;
            }
        }

        public void LeaveLobby()
        {
            try
            {
                Debug.Log("Client leaved the lobby");
                UserInLobby = false;

                foreach (var user in inLobby.Values)
                {
                    Destroy(user);
                }
                inLobby.Clear();
                currentLobby.Leave();
                OnLobbyLeave.Invoke();
            }
            catch
            {
                Debug.Log("Not Working!");
            }
        }

        private async Task<bool> CreatePlayer(SteamId id, string name)
        {
            Texture2D texture2D = await SteamFriendsManager.GetTextureFromSteamIdAsync(id);

            PlayerInfo playerInfo = new PlayerInfo(id, name, texture2D);
            GameObject obj = Instantiate(InLobbyFriend, content);
            obj.GetComponent<LobbyFriendObject>().steamid = playerInfo.SteamId;
            obj.GetComponent<LobbyFriendObject>().CheckIfOwner();
            obj.GetComponentInChildren<Text>().text = name;
            obj.GetComponentInChildren<RawImage>().texture = playerInfo.Texture;

            if (!inLobby.TryAdd(playerInfo.SteamId, obj))
            {
                Destroy(obj);
                return false;
            }

            playerInfos.Add(playerInfo.SteamId, playerInfo);
            return true;
        }

    }
}
