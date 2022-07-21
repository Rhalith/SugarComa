using Steamworks;
using UnityEngine;
public class PlayerInfo
{
    public PlayerInfo(SteamId steamId, string name, Texture2D texture)
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