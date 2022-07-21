using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Networking
{
    public class PlayerData
    {
        public PlayerData(SteamId steamId, string name, Texture2D texture)
        {
            SteamId = steamId;
            Name = name;
            Texture = texture;
        }

        public SteamId SteamId { get; set; }
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
    }


    public class SteamLobbyManager : MonoBehaviour
    {
        private static SteamLobbyManager _instance;
        public static SteamLobbyManager Instance => _instance;

        public UnityEngine.UI.Image panelImage;
        public static SteamManager steamManager;
        public static Lobby currentLobby;
        public static bool UserInLobby;
        public UnityEvent OnLobbyCreated;
        public UnityEvent OnLobbyJoined;
        public UnityEvent OnLobbyLeave;
    
        public GameObject InLobbyFriend;
    
        private Friend lobbyPartner;
        private GameObject localClientObj;
        public Friend LobbyPartner { get => lobbyPartner; set => lobbyPartner = value; }
    
        public Transform content;

        public Dictionary<SteamId, PlayerData> playerInfos = new Dictionary<SteamId, PlayerData>();
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
            SteamServerManager.OnMessageReceived += this.SteamServerManager_OnMessageReceived;
        }

        private void Start()
        {
            SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallBack;

            SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallBack;
            SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoinedCallBack;
            SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberDisconnectedCallBack;
            SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberDisconnectedCallBack;
            SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequestCallBack;
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
            SteamServerManager.SendingMessageToAll(Encoding.UTF8.GetBytes("Ready"));
        }
        
        // Bu değişiklikler için observer pattern kullanabilir miyiz?

        public void SendUnreadyToAll()
        {
            SteamServerManager.SendingMessageToAll(Encoding.UTF8.GetBytes("Unready"));
        }
        
        private void SteamServerManager_OnMessageReceived(SteamId steamid, byte[] data)
        {
            string message = Encoding.UTF8.GetString(data);
            if (message == "Ready")
            {
                SteamServerManager.SendingMessages(steamid, Encoding.UTF8.GetBytes("Ok"));
            }
            else if (message == "Unready")
            {
                SteamServerManager.SendingMessages(steamid, Encoding.UTF8.GetBytes("Ok"));
            }
            else if (message == "Ok")
            {
                if(panelImage.color != UnityEngine.Color.green)
                    panelImage.color = UnityEngine.Color.green;
                else
                    panelImage.color = UnityEngine.Color.red;
                
                // Check if everybody ready, then start a count down, then load scene
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
            else
            {
                // This was hacky, I didn't have clean way of getting lobby host steam id when joining lobby from game invite from friend 
                foreach (Friend friend in SteamFriends.GetFriends())
                {
                    if (friend.Id == id)
                    {
                        lobbyPartner = friend;
                        AcceptP2P(friend.Id);
                        break;
                    }
                }
                currentLobby = joinedLobby;
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
            Debug.Log("Client joined the lobby");
            UserInLobby = true;
            foreach (var user in inLobby.Values)
            {
                Destroy(user);
            }
            inLobby.Clear();

            await CreatePlayer(steamManager.PlayerSteamId, SteamClient.Name);

            List<Task<bool>> tasks = new List<Task<bool>>(currentLobby.MemberCount - 1);
            foreach (var friend in currentLobby.Members)
            {
                if (friend.Id != SteamClient.SteamId)
                {
                    tasks.Add(CreatePlayer(friend.Id, friend.Name));
                }
            }
            Task.WaitAll(tasks.ToArray());
            OnLobbyJoined.Invoke();
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

                Destroy(localClientObj);
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

            PlayerData playerInfo = new PlayerData(id, name, texture2D);
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
