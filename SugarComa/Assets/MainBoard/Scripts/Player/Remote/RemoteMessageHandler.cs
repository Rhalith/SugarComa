using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.Utils;
using UnityEngine;

public class RemoteMessageHandler : MonoBehaviour
{
    public RemoteScriptKeeper[] scriptKeepers;

    private void Awake()
    {
        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    private void OnDestroy()
    {
        SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
    }

    private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
    {
        if (NetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
        {
            // Player'larýn kaymasýný engellemek için önceki gönderilen pozisyona eriþtiðinde player yeni pozisyon hedef olarak alýnsýn.
            // Bunu mesajý gönderdiðimiz yerde yapabiliriz belki
            if (networkData.type == MessageType.InputDown)
            {
                scriptKeepers[NetworkManager.Instance.Index].remotePlayerMovement.UpdatePosition(networkData.position);
            }
        }
        else if (NetworkHelper.TryGetAnimationData(buffer, out AnimationStateData animationStateData))
        {
            scriptKeepers[NetworkManager.Instance.Index].playerAnimation.UpdateAnimState(animationStateData.animBoolHash);
        }
        else if (NetworkHelper.TryGetPlayerSpecData(buffer, out PlayerSpecNetworkData playerSpecData))
        {
            scriptKeepers[NetworkManager.Instance.Index].playerCollector.UpdateSpecs(playerSpecData.gold, playerSpecData.health, playerSpecData.goblet);
        }
    }
}
