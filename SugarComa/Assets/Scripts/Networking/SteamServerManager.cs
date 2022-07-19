using System.Runtime.InteropServices;
using System.Text;
using Steamworks;
using TempScripts;
using UnityEngine;

namespace Networking
{
    public class SteamServerManager : MonoBehaviour
    {
        public delegate void MessageReceivedHandler(SteamId steamid, byte[] data);
        public static event MessageReceivedHandler OnMessageReceived;

        public static SteamLobbyManager steamLobbyManager;
        public GameObject steamManager;
    
        void Awake()
        {
            DontDestroyOnLoad(this);
            // Check every 0.05 seconds for new packets
            InvokeRepeating(nameof(ReceivingMessages), 0f, 0.05f);
            steamLobbyManager = steamManager.GetComponent<SteamLobbyManager>();
        }
    
        //public static void CreateServer()
        //{
        //    if (steamLobbyManager.inLobby.Count > 0)
        //    {

        //        foreach (var id in steamLobbyManager.inLobby.Keys)
        //        {
        //            SteamNetworking.OnP2PSessionRequest = (id) =>
        //            {
        //                // If we want to let this steamid talk to us
        //                SteamNetworking.AcceptP2PSessionWithUser(id);
        //            };
        //        }
        //    }
        //}
    
        public static void SendingMessages(SteamId targetSteamId, byte[] mydata)
        {
            if (SteamLobbyManager.UserInLobby)
            {
                var sent = SteamNetworking.SendP2PPacket( targetSteamId, mydata );
            
                Debug.Log($"Sending\nOwner: {SteamLobbyManager.currentLobby.Owner.Id}" +
                          $"LobbyPartner: {steamLobbyManager.LobbyPartner.Id}" +
                          $"target: {targetSteamId}");
            }
        }
    
        // Receiving data packages
        void ReceivingMessages()
        {
            while ( SteamNetworking.IsP2PPacketAvailable() )
            {
                var packet = SteamNetworking.ReadP2PPacket();
                if (packet != null && packet.HasValue)
                {
                    OnMessageReceived?.Invoke(packet.Value.SteamId, packet.Value.Data);

                    Debug.Log($"Receiving\nOwner: {SteamLobbyManager.currentLobby.Owner.Id}" +
                              $"LobbyPartner: {steamLobbyManager.LobbyPartner.Id}" +
                              $"target: {packet.Value.SteamId}");
                    HandleMessageFrom( packet.Value.SteamId, packet.Value.Data );
                }
            }
        }

        void HandleMessageFrom(SteamId steamId, byte[] data)
        {
            // for string test 
            string message = Encoding.UTF8.GetString(data);
            Debug.Log($"user {steamId}'s message is {message}");
            
            /*
            // for struct test 
            TempStructScript.TempStruct temp = Deserialize<TempStructScript.TempStruct>(data);
            Debug.Log($"User {temp.id} move to {temp.dirStr}");
            PlayerMovement.SetDirection(temp.direction);
            */
        }
    
        public static byte[] Serialize<T>(T s)
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

        public static T Deserialize<T>(byte[] array)
            where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            var s = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return s;
        }
    }
}
