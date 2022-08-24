using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Networking.Utils
{
    public enum MessageType : byte
    {
        Ready,
        UnReady, 
        ReadyCheck,
        StartGame,
        InputDown,
        TurnOver,
        UpdateQueue,
        CreateChest,
        AnimationStateUpdate,
        Exit
    }

    public static class NetworkId
    {
        public static readonly int NetworkDataId = Animator.StringToHash("NetworkData");
        public static readonly int PlayerListNetworkDataId = Animator.StringToHash("PlayerListNetworkData");
        public static readonly int TurnNetworkDataId = Animator.StringToHash("TurnNetworkData");
        public static readonly int ChestNetworkDataId = Animator.StringToHash("ChestNetworkData");
        public static readonly int AnimationStateNetworkDataId = Animator.StringToHash("AnimationStateData");
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NetworkData
    {
        public readonly int id;
        public MessageType type;
        public Vector3 position;
        public Quaternion rotation;

        public NetworkData(MessageType type)
        {
            id = NetworkId.NetworkDataId;

            this.type = type;
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }

        public NetworkData(MessageType type, Vector3 position)
        {
            id = NetworkId.NetworkDataId;

            this.type = type;
            this.position = position;
            rotation = Quaternion.identity;
        }

        public NetworkData(MessageType type, Vector3 position, Quaternion rotation)
        {
            id = NetworkId.NetworkDataId;

            this.type = type;
            this.position = position;
            this.rotation = rotation;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 80)]
    public struct PlayerListNetworkData
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 64)]
        [FieldOffset(0)] public byte[] playerList;
        [FieldOffset(64)] public readonly int id;
        [FieldOffset(68)] public MessageType type;

        public PlayerListNetworkData(MessageType type)
        {
            id = NetworkId.PlayerListNetworkDataId;

            this.type = type;
            playerList = new byte[Marshal.SizeOf<ulong>() * SteamLobbyManager.MemberCount];
        }

        public PlayerListNetworkData(MessageType type, byte[] steamId)
        {
            id = NetworkId.PlayerListNetworkDataId;

            this.type = type;
            playerList = steamId;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TurnNetworkData
    {
        public int id;
        public byte index;
        public MessageType messageType;

        public TurnNetworkData(byte index, MessageType messageType)
        {
            id = NetworkId.TurnNetworkDataId;
            this.index = index;
            this.messageType = messageType;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ChestNetworkData
    {
        public int id;
        public byte index;
        public MessageType messageType;

        public ChestNetworkData(byte index, MessageType messageType)
        {
            id = NetworkId.ChestNetworkDataId;
            this.index = index;
            this.messageType = messageType;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AnimationStateData
    {
        public int id;
        public int playerIndex;
        public int animBoolHash;
        public MessageType messageType;

        public AnimationStateData(int animBoolHash, MessageType messageType)
        {
            id = NetworkId.AnimationStateNetworkDataId;
            playerIndex = NetworkManager.Instance.Index;
            this.animBoolHash = animBoolHash;
            this.messageType = messageType;
        }
    }
}