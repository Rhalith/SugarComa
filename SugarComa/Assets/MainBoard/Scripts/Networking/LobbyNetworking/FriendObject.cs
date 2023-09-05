using UnityEngine;
using Steamworks;

namespace Assets.MainBoard.Scripts.Networking.LobbyNetworking
{
    public class FriendObject : MonoBehaviour
    {
        public SteamId steamid;

        public async void InviteAsync()
        {
            if (SteamLobbyManager.UserInLobby)
            {
                SteamLobbyManager.currentLobby.InviteFriend(steamid);
                Debug.Log("Invited " + steamid);
            }
            else
            {
                bool result = await SteamLobbyManager.CreateLobby();
                if (result)
                {
                    SteamLobbyManager.currentLobby.InviteFriend(steamid);
                    Debug.Log("Invited " + steamid + " Created a new lobby");
                }
            }
        }
    }
}