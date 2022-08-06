using Steamworks;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Networking.Utils
{
    public class LobbyPlayerInfo
    {
        public LobbyPlayerInfo(SteamId steamId, string name, Texture2D texture)
        {
            SteamId = steamId;
            Name = name;
            Texture = texture;
        }

        public SteamId SteamId { get; set; }
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsReady { get; set; }
    }
}