using Steamworks;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
    [DefaultExecutionOrder(-100)]
    public class SteamServerManager : MonoBehaviour
    {
        public static SteamServerManager _instance;
        public static SteamServerManager Instance => _instance;

        public delegate void MessageReceivedHandler(SteamId steamid, byte[] buffer);
        public delegate void GameStartedHandler();
        public event MessageReceivedHandler OnMessageReceived;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
            InvokeRepeating(nameof(ReceivingMessages), 0, 0.05f);
        }
    
        public bool SendingMessage(SteamId targetSteamId, byte[] buffer)
        {
            if (SteamLobbyManager.UserInLobby)
            {
                return SteamNetworking.SendP2PPacket(targetSteamId, buffer);
            }
            return false;
        }

        public bool SendingMessageToAll(byte[] buffer)
        {
            if (SteamLobbyManager.UserInLobby)
            {
                bool[] response = new bool[SteamLobbyManager.Instance.MemberCount - 1];

                int index = 0;
                foreach (var id in SteamLobbyManager.Instance.playerInfos.Keys)
                {
                    if (id != SteamManager.Instance.PlayerSteamId)
                    {
                        response[index] = SteamNetworking.SendP2PPacket(id, buffer);
                        index++;
                    }
                }

                return response.All(res => res);
            }
            return false;
        }
    
        // Receiving data packages
        private void ReceivingMessages()
        {
            try
            {
                while (SteamNetworking.IsP2PPacketAvailable())
                {
                    var packet = SteamNetworking.ReadP2PPacket();
                    if (packet != null && packet.HasValue)
                    {
                        OnMessageReceived?.Invoke(packet.Value.SteamId, packet.Value.Data);
                        //HandleMessageFrom(packet.Value.SteamId, packet.Value.Data);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        // paramter: , bool makeSureSendAll = false
        //if (makeSureSendAll)
        //{
        //    while (response.Any(res => !res))
        //    {
        //        for (int i = 0; i < response.Length; i++)
        //        {
        //            if (!response[i])
        //            {
        //                var element = SteamLobbyManager.Instance.playerInfos.ElementAt(i);
        //                response[i] = SteamNetworking.SendP2PPacket(element.Key, buffer);
        //            }
        //        }
        //    }
        //}

        //void HandleMessageFrom(SteamId steamId, byte[] data)
        //{
        //    /*
        //    // for string test 
        //    string message = Encoding.UTF8.GetString(data);
        //    Debug.Log($"user {steamId}'s message is {message}");
        //    PlayerMovement.SetDirection(Vector3.left);
        //    */

        //    // for struct test 
        //    //PlayerInfo.Info playerInfo = Deserialize<PlayerInfo.Info>(data);
        //    //Debug.Log($"User {playerInfo.id} move to {playerInfo.dirStr}");
        //    //PlayerMovement.SetDirection(playerInfo.direction);
        //}
    }
}
