using System.Runtime.InteropServices;

namespace Assets.MainBoard.Scripts.Networking.Utils
{
    public static class NetworkHelper
    {
        public static byte[] Serialize<T>(in T s)
            where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));

            var array = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(s, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);

            return array;
        }

        public static void Deserialize<T>(byte[] array, out T data)
            where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            data = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
        }

        public static byte[] SteamIdToByteArray(Steamworks.SteamId[] idArr)
        {
            int size = Marshal.SizeOf<ulong>();
            byte[] bytes = new byte[size * SteamLobbyManager.MemberCount];

            for (int i = 0; i < SteamLobbyManager.MemberCount; i++)
            {
                byte[] tempByteArr = System.BitConverter.GetBytes((ulong)idArr[i]);
                System.Array.Copy(tempByteArr, 0, bytes, size * i, size);
            }

            return bytes;
        }

        public static Steamworks.SteamId[] ByteArrayToSteamId(byte[] bytes)
        {
            int size = Marshal.SizeOf<ulong>();
            Steamworks.SteamId[] idArr = new Steamworks.SteamId[SteamLobbyManager.MemberCount];

            for (int i = 0; i < SteamLobbyManager.MemberCount; i++)
            {
                byte[] tempArr = new byte[size];
                System.Array.Copy(bytes, size * i, tempArr, 0, size);
                idArr[i] = System.BitConverter.ToUInt64(tempArr);
            }
            return idArr;
        }

        public static bool TryGetNetworkData(byte[] buffer, out NetworkData networkData)
        {
            if (buffer.Length != Marshal.SizeOf<NetworkData>())
            {
                networkData = new NetworkData();
                return false;
            }

            Deserialize(buffer, out networkData);
            if (networkData.id != NetworkId.NetworkDataId)
            {
                return false;
            }
            return true;
        }

        public static bool TryGetPlayerListData(byte[] buffer, out PlayerListNetworkData playerListData)
        {
            if (buffer.Length != Marshal.SizeOf<PlayerListNetworkData>())
            {
                playerListData = new PlayerListNetworkData();
                return false;
            }

            Deserialize(buffer, out playerListData);
            if (playerListData.id != NetworkId.PlayerListNetworkDataId)
            {
                return false;
            }
            return true;
        }

        public static bool TryGetTurnNetworkData(byte[] buffer, out TurnNetworkData data)
        {
            if (buffer.Length != Marshal.SizeOf<TurnNetworkData>())
            {
                data = new TurnNetworkData();
                return false;
            }

            Deserialize(buffer, out data);
            if (data.id != NetworkId.TurnNetworkDataId)
            {
                return false;
            }
            return true;
        }

        public static bool TryGetChestData(byte[] buffer, out ChestNetworkData networkData)
        {
            if (buffer.Length != Marshal.SizeOf<ChestNetworkData>())
            {
                networkData = new ChestNetworkData();
                return false;
            }

            Deserialize(buffer, out networkData);
            if (networkData.id != NetworkId.ChestNetworkDataId)
            {
                return false;
            }
            return true;
        }

        public static bool TryGetAnimationData(byte[] buffer, out AnimationStateData networkData)
        {
            if (buffer.Length != Marshal.SizeOf<AnimationStateData>())
            {
                networkData = new AnimationStateData();
                return false;
            }

            Deserialize(buffer, out networkData);
            if (networkData.id != NetworkId.AnimationStateNetworkDataId)
            {
                return false;
            }
            return true;
        }
    }
}