using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class SteamServerManager : MonoBehaviour
{
    public delegate void MessageReceivedHandler(SteamId steamid, byte[] data);
    public static event MessageReceivedHandler OnMessageReceived;

    public static SteamLobbyManager steamLobbyManager;
    public GameObject steamManager;
    
    void Awake()
    {
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
    
    public static void SendingMessages(SteamId targetSteamId, string message)
    {
        if (SteamLobbyManager.UserInLobby)
        {
            byte[] mydata = Encoding.UTF8.GetBytes(message);

            var sent = SteamNetworking.SendP2PPacket( targetSteamId, mydata );
            
            /*
            if (!sent)
            {
                var sent2 = SteamNetworking.SendP2PPacket( targetSteamId, mydata );
                if (!sent2)
                {
                    thingsThatFailedToSend.Add(mydata);
                }
            }
            */
            
            Debug.Log($"is message sent: {sent}," +
                $"Owner: {SteamLobbyManager.currentLobby.Owner.Id}" +
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

                Debug.Log($"OnMessageReceived:" +
                    $"Owner: {SteamLobbyManager.currentLobby.Owner.Id}" +
                    $"LobbyPartner: {steamLobbyManager.LobbyPartner.Id}" +
                    $"target: {packet.Value.SteamId}");
                //HandleMessageFrom( packet.Value.SteamId, packet.Value.Data );
            }
        }
    }

}
