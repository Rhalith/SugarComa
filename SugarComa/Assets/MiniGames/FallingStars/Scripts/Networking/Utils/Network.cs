using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Networking.Utils
{
    public enum MessageType : byte
    {
        Move,
        Rotate,
        EndMiniGame,
        AnimationStateUpdate,
        UpdatePlayerSpecs,
        Exit
    }

    public static class NetworkId
    {
        public static readonly int NetworkDataId = Animator.StringToHash("NetworkData");
        public static readonly int AnimationStateNetworkDataId = Animator.StringToHash("AnimationStateData");
        public static readonly int PlayerSpecDataId = Animator.StringToHash("PlayerSpecData");
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NetworkData
    {
        public readonly int id;
        public MessageType type;
        public Vector3 moveDir;
        public Vector3 rotationDir;

        public NetworkData(MessageType type)
        {
            id = NetworkId.NetworkDataId;

            this.type = type;
            moveDir = Vector3.zero;
            rotationDir = Vector3.zero;
        }

        public NetworkData(MessageType type, Vector3 moveDir)
        {
            id = NetworkId.NetworkDataId;

            this.type = type;
            this.moveDir = moveDir;
            rotationDir = Vector3.zero;
        }

        public NetworkData(MessageType type, Vector3 moveDir, Vector3 rotationDir)
        {
            id = NetworkId.NetworkDataId;

            this.type = type;
            this.moveDir = moveDir;
            this.rotationDir = rotationDir;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AnimationStateData
    {
        public int id;
        //public int playerIndex;
        public int animBoolHash;

        public AnimationStateData(int animBoolHash)
        {
            id = NetworkId.AnimationStateNetworkDataId;
           //playerIndex = NetworkManager.Instance.Index;
            this.animBoolHash = animBoolHash;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerSpecNetworkData
    {
        public int id;
        public byte health;

        public PlayerSpecNetworkData(byte health)
        {
            id = NetworkId.AnimationStateNetworkDataId;
            this.health = health;
        }
    }

}