using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Networking.Utils
{
    public enum MessageType2 : byte
    {
        Ready,
        UnReady,
        ReadyCheck,
        StartGame,
        InputDown,
        TurnOver,
        CreatePlayers,
        UpdatePlayers,
        CreateChest,
        AnimationStateUpdate,
        UpdatePlayerSpecs,
        GoToMinigame,
        Exit
    }

    public static class NetworkId2
    {
        public static readonly int NetworkDataId = Animator.StringToHash("NetworkData");
        public static readonly int PlayerListNetworkDataId = Animator.StringToHash("PlayerListNetworkData");
        public static readonly int TurnNetworkDataId = Animator.StringToHash("TurnNetworkData");
        public static readonly int ChestNetworkDataId = Animator.StringToHash("ChestNetworkData");
        public static readonly int AnimationStateNetworkDataId = Animator.StringToHash("AnimationStateData");
        public static readonly int PlayerSpecDataId = Animator.StringToHash("PlayerSpecData");
        public static readonly int MiniGameNetworkDataId = Animator.StringToHash("MiniGameNetworkData");
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NetworkData2
    {
        // ID - Message Type - Pos - Rotation - playerList -
        // -| TurnEnd? |- MinigameId - chestIndex - playerIndex
        // - animBoolHash -
        // - ?Gold - Health - Goblet amounts

        // For TurnEnd Id is enough

        /*
        _buffer[0 - 4] = id;                // 1 int -> 4 byte
        _buffer[4 - 16] = pos;              // 3 float -> 12 byte
        _buffer[16 - 32] = rotation;        // 4 float -> 16 byte
        _buffer[32 - 96] = playerList;      // 1 int -> 4 byte
        _buffer[96 - 100] = MinigameId;     // 1 int -> 4 byte
        _buffer[100 - 104] = ChestIndex;    // 1 int -> 4 byte
        _buffer[104 - 108] = PlayerIndex;   // 1 int -> 4 byte
        _buffer[108 - 112] = AnimBoolHash;  // 1 int -> 4 byte

        // ???
        _buffer[112 - 113] = gold;          // 1 byte
        _buffer[113 - 114] = health;        // 1 byte
        _buffer[114 - 115] = goblet;        // 1 byte


        public readonly int id;
        public MessageType type;
        public Vector3 position;
        public Quaternion rotation;
        public byte[] playerList;
        public byte minigameIndex;
        public byte chestIndex;
        public byte playerIndex;
        public int animBoolHash;
        */

        public byte[] _buffer;

        /*
        // NetworkId static diye parametre olarak kullanýlamýyor...
        // Bunun yerine messagetype kullanýlabilir?
        public void ChangeMessageId(NetworkId id)
        {
            this.id = id;
        }
        */

        public void AddPosition(in Vector3 pos)
        {
            Serialize(pos, 4);
        }

        public void AddRotation(in Quaternion rot)
        {
            Serialize(rot, 16);
        }
        
        public void AddPlayerList(Steamworks.SteamId[] playerList)
        {
            int length = Marshal.SizeOf<ulong>() *  SteamLobbyManager.MemberCount;

            byte[] tempArr = NetworkHelperNew.SteamIdToByteArray(playerList);
            // Temp'i buffer'a at
            System.Array.Copy(tempArr, 0, _buffer, 32, length);
        }
        
        public void AddMinigameIndex(int index)
        {
            Serialize(index, 96);
        }

        public void AddChestIndex(int index)
        {
            Serialize(index, 100);
        }

        public void AddPlayerIndex(int index)
        {
            Serialize(index, 104);
        }

        public void AddAnimationHash(int hash)
        {
            Serialize(hash, 108);
        }

        public void Serialize<T>(in T s, int startIndex)
        {
            var size = Marshal.SizeOf(typeof(T));

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(s, ptr, true);
            Marshal.Copy(ptr, _buffer, startIndex, size);
            Marshal.FreeHGlobal(ptr);
        }
    }
}
