using System.Runtime.InteropServices;
using UnityEngine;

public enum MessageType : byte
{
    Ready,
    UnReady,
    StartGame,
    InputDown,
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
    
    public NetworkData(MessageType type)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        position = Vector3.zero;
    }

    public NetworkData(MessageType type, Vector3 position)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        this.position = position;
    }
}