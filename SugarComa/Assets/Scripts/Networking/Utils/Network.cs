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
}

[StructLayout(LayoutKind.Sequential)]
public struct NetworkData
{
    public readonly int id;
    public MessageType type;
    public Vector3 position;
    public Quaternion rotation;
    //public Steamworks.SteamId[] playerIdArr;

    public NetworkData(MessageType type)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        position = Vector3.zero;
        rotation = Quaternion.identity;
        //playerIdArr = new Steamworks.SteamId[1];
    }

    public NetworkData(MessageType type, Vector3 position)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        this.position = position;
        rotation = Quaternion.identity;
        //playerIdArr = new Steamworks.SteamId[1];
    }

    public NetworkData(MessageType type, Vector3 position, Quaternion rotation)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        this.position = position;
        this.rotation = rotation;
        //playerIdArr = new Steamworks.SteamId[1];
    }

    public NetworkData(MessageType type, List<Steamworks.SteamId> _playerIdList)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        position = Vector3.zero;
        rotation = Quaternion.identity;
        //playerIdArr = _playerIdList.ToArray();
    }
}