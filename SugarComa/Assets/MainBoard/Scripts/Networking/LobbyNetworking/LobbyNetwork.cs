using UnityEngine;
using System.Runtime.InteropServices;

namespace Assets.MainBoard.Scripts.Networking.LobbyNetworking
{
    public enum LobbyMessageType : byte
    {
        Ready,
        UnReady,
        ReadyCheck,
        StartGame
    }

    public static class LobbyNetworkId
    {
        public static readonly int LobbyDataId = Animator.StringToHash("LobbyData");
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LobbyData
    {
        public LobbyMessageType type;
        public int id;

        public LobbyData(LobbyMessageType messageType)
        {
            id = LobbyNetworkId.LobbyDataId;
            type = messageType;
        }
    }
}