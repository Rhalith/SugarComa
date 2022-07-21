using System.Runtime.InteropServices;
using System.Text;
using Steamworks;
using TempScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        
        // For now i'll use a button instead of check if everybody ready.
        public static void CreateServer()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    
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
        
        public static void SendingMessageToAll(byte[] mydata)
        {
            if (SteamLobbyManager.UserInLobby)
            {
                foreach (var id in steamLobbyManager.inLobby.Keys)
                {
                    var sent = SteamNetworking.SendP2PPacket( id, mydata );
            
                    Debug.Log($"Sending\nOwner: {SteamLobbyManager.currentLobby.Owner.Id}" +
                              $"LobbyPartner: {steamLobbyManager.LobbyPartner.Id}" +
                              $"target: {id}");
                }
            }
        }
    
        // Receiving data packages
        void ReceivingMessages()
        {
            try
            {
                while (SteamNetworking.IsP2PPacketAvailable())
                {
                    var packet = SteamNetworking.ReadP2PPacket();
                    if (packet != null && packet.HasValue)
                    {
                        OnMessageReceived?.Invoke(packet.Value.SteamId, packet.Value.Data);

                        Debug.Log($"Receiving\nOwner: {SteamLobbyManager.currentLobby.Owner.Id}" +
                                  $"LobbyPartner: {steamLobbyManager.LobbyPartner.Id}" +
                                  $"target: {packet.Value.SteamId}");
                        HandleMessageFrom(packet.Value.SteamId, packet.Value.Data);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        void HandleMessageFrom(SteamId steamId, byte[] data)
        {
            /*
            // for string test 
            string message = Encoding.UTF8.GetString(data);
            Debug.Log($"user {steamId}'s message is {message}");
            PlayerMovement.SetDirection(Vector3.left);
            */
            
            // for struct test 
            PlayerInfo.Info playerInfo = Deserialize<PlayerInfo.Info>(data);
            Debug.Log($"User {playerInfo.id} move to {playerInfo.dirStr}");
            PlayerMovement.SetDirection(playerInfo.direction);
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
