using System.Collections;
using System.Collections.Generic;
using Networking;
using UnityEngine;
using Steamworks;
using UnityEngine.UIElements;

public class LobbyFriendObject : MonoBehaviour
{
    public SteamId steamid;
    public bool isOwner = false;
    public bool isReady = false;

    public void CheckIfOwner()
    {
        if (steamid == SteamLobbyManager.currentLobby.Owner.Id)
        {
            isOwner = true;
        }
    }

    public async void KickAsync()
    {
        // Kick Function
        Debug.Log("Kicked " + steamid);
    }
    
}
