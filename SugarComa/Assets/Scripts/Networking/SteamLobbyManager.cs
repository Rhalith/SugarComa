﻿using Steamworks;
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
    
    private Friend lobbyPartner;
    private GameObject localClientObj;
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

        SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallBack;

        SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallBack;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoinedCallBack;
        SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberDisconnectedCallBack;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberDisconnectedCallBack;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequestCallBack;
        // SceneManager.sceneLoaded += OnSceneLoaded
    }

    public void SendToReady()
    {
        SteamServerManager.SendingMessages(currentLobby.Owner.Id, "Ready");
    }

    void Update()
    {
        SteamClient.RunCallbacks();
    }

    private async void OnLobbyMemberJoinedCallBack(Lobby lobby, Friend friend)
    {
        Debug.Log($"{friend.Name} joined the lobby");
        GameObject obj = Instantiate(InLobbyFriend, content);
        obj.GetComponentInChildren<Text>().text = friend.Name;
        obj.GetComponentInChildren<RawImage>().texture = await SteamFriendsManager.GetTextureFromSteamIdAsync(friend.Id);
        
        if(inLobby.TryAdd(friend.Id, obj))
        {
            AcceptP2P(friend.Id);
        }
        else
        {
            Destroy(obj);
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

    void OnLobbyEnteredCallBack(Lobby lobby)
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

        List<Task<Texture2D>> tasks = new List<Task<Texture2D>>(currentLobby.MemberCount - 1);
        // TODO remove. use steam friends manager pp.
        tasks.Add(SteamFriendsManager.GetTextureFromSteamIdAsync(SteamClient.SteamId));

        localClientObj = obj;

        foreach (var friend in currentLobby.Members)
        {
            if (friend.Id != SteamClient.SteamId)
            {
                GameObject obj2 = Instantiate(InLobbyFriend, content);
                obj2.GetComponentInChildren<Text>().text = friend.Name;
                tasks.Add(SteamFriendsManager.GetTextureFromSteamIdAsync(friend.Id));
                inLobby.TryAdd(friend.Id, obj2);
            }
        }
        Task.WaitAll(tasks.ToArray());

        int i = 1;
        obj.GetComponentInChildren<RawImage>().texture = tasks[0].Result;
        foreach (var member in inLobby)
        {
            member.Value.GetComponentInChildren<RawImage>().texture = tasks[i].Result;
            i++;
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
}
