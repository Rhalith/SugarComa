using System.Runtime.InteropServices;

namespace Assets.MiniGames.FallingStars.Scripts.Networking.Utils
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

        public static bool TryGetPlayerSpecData(byte[] buffer, out PlayerSpecNetworkData networkData)
        {
            if (buffer.Length != Marshal.SizeOf<PlayerSpecNetworkData>())
            {
                networkData = new PlayerSpecNetworkData();
                return false;
            }

            Deserialize(buffer, out networkData);
            if (networkData.id != NetworkId.PlayerSpecDataId)
            {
                return false;
            }
            return true;
        }
    }
}