using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum MessageType : byte
{
    Ready,
    UnReady,
    StartGame,
    InputDown,
    TurnOver,
    UpdateQueue,
    Exit
}

public static class NetworkId
{
    public static readonly int NetworkDataId = Animator.StringToHash("NetworkData");
    public static readonly int PlayerListNetworkDataId = Animator.StringToHash("PlayerListNetworkData");
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
    [FieldOffset(0)]public byte[] playerList;
    [FieldOffset(64)] public readonly int id;
    [FieldOffset(68)] public MessageType type;

    public PlayerListNetworkData(MessageType type)
    {
        id = NetworkId.PlayerListNetworkDataId;

        this.type = type;
        playerList = new byte[Marshal.SizeOf<ulong>() * NetworkManager.MaxPlayerCount];
    }

    public PlayerListNetworkData(MessageType type, byte[] steamId)
    {
        id = NetworkId.PlayerListNetworkDataId;

        this.type = type;
        this.playerList = steamId;
    }
}