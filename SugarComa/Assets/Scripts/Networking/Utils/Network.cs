using System.Runtime.InteropServices;
using UnityEngine;
public enum Direction : byte
{
    Left, Right, Up, Down
}

public enum MessageType : byte
{
    Ready,
    UnReady,
    InputDown
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
    public Direction direction;
    
    public NetworkData(MessageType type)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        position = Vector3.zero;
        direction = Direction.Left;
    }

    public NetworkData(MessageType type, Vector3 position, Direction direction)
    {
        id = NetworkId.NetworkDataId;

        this.type = type;
        this.position = position;
        this.direction = direction;
    }
}