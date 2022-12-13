using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using Steamworks.Data;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.MainBoard.Scripts.Networking.Utils;

namespace Assets.MainBoard.Scripts.Networking.LobbyNetworking
{
    public class SteamLobbyManager : MonoBehaviour
    {
        private static SteamLobbyManager _instance;
        public static SteamLobbyManager Instance => _instance;

        public GameObject startGameButton;
        public Transform hostContent;
        public Transform clientContent;
        public GameObject hostLeftMessageObj;

        public static SteamManager steamManager;
        public static Lobby currentLobby;
        public static bool UserInLobby;

        #region Events
        public UnityEvent OnLobbyCreated;
        public UnityEvent OnLobbyJoinedHost;
        public UnityEvent OnLobbyJoinedClient;
        public UnityEvent OnLobbyLeave;
        #endregion

        public GameObject InLobbyFriend;

        public const int MinPlayerCount = 2;
        public const int MaxPlayerCount = 8;

        #region properties
        public static int MemberCount => currentLobby.MemberCount;
        public bool AmIHost => currentLobby.Owner.Id == steamManager.PlayerSteamId;
        public bool AmIReady => playerInfos[steamManager.PlayerSteamId].IsReady;
        #endregion

        public Dictionary<SteamId, LobbyPlayerInfo> playerInfos = new Dictionary<SteamId, LobbyPlayerInfo>();
        public Dictionary<SteamId, GameObject> inLobby = new Dictionary<SteamId, GameObject>();

        private SteamId hostId;

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
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneManager_ActiveSceneChanged;
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
            if (arg1.buildIndex == 0) return;

            SceneManager.activeSceneChanged -= SceneManager_ActiveSceneChanged;

            #region Lobby Events
            SteamMatchmaking.OnLobbyCreated -= OnLobbyCreatedCallBack;
            SteamMatchmaking.OnLobbyEntered -= OnLobbyEnteredCallBack;
            SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoinedCallBack;
            SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequestCallBack;
            #endregion
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
                SteamFriendsManager.Instance.UpdateFriendListByFriend(friend.Id);
                AcceptP2P(friend.Id);
            }
        }

        void OnLobbyMemberDisconnectedCallBack(Lobby lobby, Friend friend)
        {
            Debug.Log($"{friend.Name} left the lobby");

            if (friend.Id == hostId)
            {
                LeaveLobby();
                hostLeftMessageObj.SetActive(true);
                return;
            }

            Debug.Log($"New lobby owner is {currentLobby.Owner}");

            if (AmIHost)
                startGameButton.SetActive(true);

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
                startGameButton.SetActive(true);
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
            playerInfos.Clear();

            if (AmIHost)
                OnLobbyJoinedHost.Invoke();
            else
                OnLobbyJoinedClient.Invoke();

            int count = currentLobby.MemberCount-1;

            if (count > 0)
            {
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

            await CreatePlayer(steamManager.PlayerSteamId, SteamClient.Name);

            hostId = currentLobby.Owner.Id;

            // Update ready/unready status
            if (count > 0)
                SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new LobbyData(LobbyMessageType.ReadyCheck)));
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
            catch (System.Exception exception)
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
                startGameButton.SetActive(false);
                inLobby.Clear();
                playerInfos.Clear();
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

            LobbyPlayerInfo playerInfo = new LobbyPlayerInfo(id, name, texture2D);

            GameObject obj;

            if (AmIHost)
                obj = Instantiate(InLobbyFriend, hostContent);
            else
                obj = Instantiate(InLobbyFriend, clientContent);

            obj.GetComponent<LobbyFriendObject>().steamid = playerInfo.SteamId;
            obj.GetComponent<LobbyFriendObject>().CheckIfOwner();
            obj.GetComponentInChildren<TMPro.TMP_Text>().text = name;
            obj.GetComponentInChildren<RawImage>().texture = playerInfo.Texture;

            if (!inLobby.TryAdd(playerInfo.SteamId, obj))
            {
                Destroy(obj);
                return false;
            }

            playerInfos.Add(playerInfo.SteamId, playerInfo);
            return true;
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void UpdateLobbyFriend(SteamId steamId, LobbyMessageType updateType)
        {
            switch (updateType)
            {
                case LobbyMessageType.Ready:
                    {
                        playerInfos[steamId].IsReady = true;
                        inLobby[steamId].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.green;
                    }
                    break;
                case LobbyMessageType.UnReady:
                    {
                        playerInfos[steamId].IsReady = false;
                        inLobby[steamId].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.red;
                    }
                    break;
            }
        }
    }
}
