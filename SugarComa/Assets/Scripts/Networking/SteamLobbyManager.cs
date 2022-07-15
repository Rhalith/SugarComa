using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SteamLobbyManager : MonoBehaviour
{
    public UnityEngine.UI.Image panelImage;
    public static SteamManager SteamManager;
    public static Lobby currentLobby;
    public static bool UserInLobby;
    public UnityEvent OnLobbyCreated;
    public UnityEvent OnLobbyJoined;
    public UnityEvent OnLobbyLeave;
    
    public GameObject InLobbyFriend;
    public SteamId OpponentSteamId { get; set; }
    
    private Friend lobbyPartner;
    public Friend LobbyPartner { get => lobbyPartner; set => lobbyPartner = value; }
    
    public Transform content;

    public Dictionary<SteamId, GameObject> inLobby = new Dictionary<SteamId, GameObject>();

    private void Awake()
    {
        SteamManager = GetComponent<SteamManager>();
        SteamServerManager.OnMessageReceived += this.SteamServerManager_OnMessageReceived;
    }

    private void SteamServerManager_OnMessageReceived(SteamId steamid, byte[] data)
    {
        string message = System.Text.Encoding.UTF8.GetString(data);
        if (message == "Ready")
        {
            SteamServerManager.SendingMessages(steamid, "Ok");
        }
        else if (message == "Ok")
        {
            panelImage.color = UnityEngine.Color.green;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreatedCallBack;
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallBack;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallBack;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoinedCallBack;
        SteamMatchmaking.OnChatMessage += OnChatMessageCallBack;
        SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberDisconnectedCallBack;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberDisconnectedCallBack;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequestCallBack;
        SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
        
        SteamMatchmaking.OnLobbyDataChanged += SteamMatchmaking_OnLobbyDataChanged;

        // SceneManager.sceneLoaded += OnSceneLoaded;
        // UpdateRichPresenceStatus(SceneManager.GetActiveScene().name);
    }

    public void SendToReady()
    {
        SteamServerManager.SendingMessages(currentLobby.Owner.Id, "Ready");
    }

    private void SteamMatchmaking_OnLobbyDataChanged(Lobby obj)
    {
        var x = obj.Data;
        var y = obj.Owner;
        var members = obj.Members;

        foreach (var member in members)
        {

        }
        throw new NotImplementedException();
    }

    void Update()
    {
        SteamClient.RunCallbacks();
    }
    
    void OnLobbyInvite(Friend friend, Lobby lobby)
    {
        Debug.Log($"{friend.Name} invited you to his lobby.");
    }
    
    private void OnLobbyGameCreatedCallBack(Lobby lobby, uint ip, ushort port, SteamId id)
    {

    }

    private async void OnLobbyMemberJoinedCallBack(Lobby lobby, Friend friend)
    {
        Debug.Log($"{friend.Name} joined the lobby");
        GameObject obj = Instantiate(InLobbyFriend, content);
        obj.GetComponentInChildren<Text>().text = friend.Name;
        obj.GetComponentInChildren<RawImage>().texture = await SteamFriendsManager.GetTextureFromSteamIdAsync(friend.Id);
        inLobby.Add(friend.Id, obj);
    }

    void OnLobbyMemberDisconnectedCallBack(Lobby lobby, Friend friend)
    {
        Debug.Log($"{friend.Name} left the lobby");
        Debug.Log($"New lobby owner is {currentLobby.Owner}");
        if (inLobby.ContainsKey(friend.Id))
        {
            Destroy(inLobby[friend.Id]);
            inLobby.Remove(friend.Id);
        }
    }

    void OnChatMessageCallBack(Lobby lobby, Friend friend, string message)
    {
        // Received chat message
        if (friend.Id != SteamManager.PlayerSteamId)
        {
            Debug.Log($"incoming chat message from {friend.Name} : {message}");

            // I used chat to setup game parameters on occasion like when player joined lobby with preselected playable bug family, prob not best way to do it
            // But after host received player chat message I set off the OnLobbyGameCreated callback with lobby.SetGameServer(PlayerSteamId)
            lobby.SetJoinable(false);
            lobby.SetGameServer(SteamManager.PlayerSteamId);
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
                    break;
                }
            }
            currentLobby = joinedLobby;
            OpponentSteamId = id;
            AcceptP2P(OpponentSteamId);
            //SceneManager.LoadScene("Scene to load");
            currentLobby = joinedLobby;
        }
    }
    
    private void AcceptP2P(SteamId opponentId)
    {
        try
        {
            foreach (var member in currentLobby.Members)
            {
                if (!member.IsMe)
                {
                    // For each players to send P2P packets to each other, they each must call this on the other players
                    SteamNetworking.AcceptP2PSessionWithUser(member.Id);
                }
                else
                {

                }
            }
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

        GameObject obj = Instantiate(InLobbyFriend, content);
        obj.GetComponentInChildren<Text>().text = SteamClient.Name;
        obj.GetComponentInChildren<RawImage>().texture = await SteamFriendsManager.GetTextureFromSteamIdAsync(SteamClient.SteamId);

        inLobby.Add(SteamClient.SteamId, obj);

        foreach (var friend in currentLobby.Members)
        {
            if (friend.Id != SteamClient.SteamId)
            {
                GameObject obj2 = Instantiate(InLobbyFriend, content);
                obj2.GetComponentInChildren<Text>().text = friend.Name;
                obj2.GetComponentInChildren<RawImage>().texture = await SteamFriendsManager.GetTextureFromSteamIdAsync(friend.Id);

                inLobby.Add(friend.Id, obj2);
            }
        }
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
            UserInLobby = false;
            currentLobby.Leave();
            OnLobbyLeave.Invoke();
            foreach (var user in inLobby.Values)
            {
                Destroy(user);
            }
            inLobby.Clear();
        }
        catch
        {

        }
    }


}
