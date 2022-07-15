using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class SteamServerManager : MonoBehaviour
{
    public static SteamLobbyManager steamLobbyManager;
    public GameObject steamManager;
    
    void Awake()
    {
        // Check every 0.05 seconds for new packets
        InvokeRepeating(nameof(ReceivingMessages), 0f, 0.05f);
        steamLobbyManager = steamManager.GetComponent<SteamLobbyManager>();
    }
    
    public static void CreateServer()
    {
        if (steamLobbyManager.inLobby.Count > 0)
        {
            foreach (var user in steamLobbyManager.inLobby.Values)
            {
                Debug.Log(user.name);
            }
        }
    }
    
    static void SendingMessages(SteamId targetSteamId, string message)
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
            
            Debug.Log($"is message sent: {sent}");
        }
    }
    
    // Receiving data packages
    void ReceivingMessages()
    {
        SteamNetworking.OnP2PSessionRequest = (steamid) =>
        {
            // If we want to let this steamid talk to us
            SteamNetworking.AcceptP2PSessionWithUser(steamid);
        };
        
        while ( SteamNetworking.IsP2PPacketAvailable() )
        {
            var packet = SteamNetworking.ReadP2PPacket();
            if ( packet.HasValue )
            {
                HandleMessageFrom( packet.Value.SteamId, packet.Value.Data );
            }
        }
    }
    
    static void HandleMessageFrom( SteamId steamid, byte[] data )
    {
        Debug.Log(steamid);
    }
    
    
}
