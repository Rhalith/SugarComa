using Assets.MainBoard.Scripts.Networking.Utils;
using System.Runtime.InteropServices;

namespace Assets.MainBoard.Scripts.Networking.MainBoardNetworking
{
    public static class MBNetworkHelper
    {
        public static bool TryGetNetworkData(byte[] buffer, out NetworkData networkData)
        {
            if (buffer.Length != Marshal.SizeOf<NetworkData>())
            {
                networkData = new NetworkData();
                return false;
            }

            NetworkHelper.Deserialize(buffer, out networkData);
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

            NetworkHelper.Deserialize(buffer, out playerListData);
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

            NetworkHelper.Deserialize(buffer, out data);
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

            NetworkHelper.Deserialize(buffer, out networkData);
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

            NetworkHelper.Deserialize(buffer, out networkData);
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

            NetworkHelper.Deserialize(buffer, out networkData);
            if (networkData.id != NetworkId.PlayerSpecDataId)
            {
                return false;
            }
            return true;
        }

        public static bool TryGetMiniGameNetworkData(byte[] buffer, out MiniGameNetworkData networkData)
        {
            if (buffer.Length != Marshal.SizeOf<MiniGameNetworkData>())
            {
                networkData = new MiniGameNetworkData();
                return false;
            }

            NetworkHelper.Deserialize(buffer, out networkData);
            if (networkData.id != NetworkId.MiniGameNetworkDataId)
            {
                return false;
            }
            return true;
        }
    }
}