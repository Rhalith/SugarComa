using Assets.MainBoard.Scripts.Networking.LobbyNetworking;
using Assets.MainBoard.Scripts.Networking.Utils;
using System.Runtime.InteropServices;
using UnityEngine;

public class LobbyNetworkHelper : MonoBehaviour
{
    public static bool TryGetLobbyData(byte[] buffer, out LobbyData lobbyData)
    {
        if (buffer.Length != Marshal.SizeOf<LobbyData>())
        {
            lobbyData = new LobbyData();
            return false;
        }

        NetworkHelper.Deserialize(buffer, out lobbyData);
        if (lobbyData.id != LobbyNetworkId.LobbyDataId)
        {
            return false;
        }
        return true;
    }
}
