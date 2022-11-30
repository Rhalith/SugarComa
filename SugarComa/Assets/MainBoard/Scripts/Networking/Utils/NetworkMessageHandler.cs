using Steamworks;
using UnityEngine;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Player.Handlers;
using Assets.MainBoard.Scripts.Networking.Utils;

public class NetworkMessageHandler : MonoBehaviour
{
    private RemoteScriptKeeper[] _scriptKeepers;
    private int localIndex;

    private void Awake()
    {
        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    private void OnDestroy()
    {
        SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
    }

    private void OnMessageReceived(SteamId steamId, byte[] buffer)
    {
        if (IsNetworData(steamId, buffer)) return;
        if (IsAnimationStateData(buffer)) return;
        if (IsPlayerSpecNetworkData(buffer)) return;
        if (IsPlayerListNetworkData(buffer)) return;
    }

    private bool IsNetworData(SteamId steamId, byte[] buffer)
    {
        if (NetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
        {
            // Player'larýn kaymasýný engellemek için önceki gönderilen pozisyona eriþtiðinde player yeni pozisyon hedef olarak alýnsýn.
            // Bunu mesajý gönderdiðimiz yerde yapabiliriz belki
            if (networkData.type == MessageType.InputDown)
            {
                int keeperIndex = PlayerTurnHandler.Index > localIndex ? PlayerTurnHandler.Index - 1 : PlayerTurnHandler.Index;
                _scriptKeepers[keeperIndex].remotePlayerMovement.UpdatePosition(networkData.position);
            }
            else if (networkData.type == MessageType.Exit)
            {
                var player = PlayerTurnHandler.GetPlayer(steamId);
                if (player != null)
                {
                    Destroy(gameObject);
                    PlayerTurnHandler.RemovePlayer(steamId);
                    SteamLobbyManager.Instance.playerInfos.Remove(steamId);
                }
            }
            return true;
        }

        return false;
    }

    private bool IsAnimationStateData(byte[] buffer)
    {
        if (NetworkHelper.TryGetAnimationData(buffer, out AnimationStateData animationStateData))
        {
            int keeperIndex = PlayerTurnHandler.Index > localIndex ? PlayerTurnHandler.Index - 1 : PlayerTurnHandler.Index;
            _scriptKeepers[keeperIndex].playerAnimation.UpdateAnimState(animationStateData.animBoolHash);
            return true;
        }
        return false;
    }

    private bool IsPlayerSpecNetworkData(byte[] buffer)
    {
        if (NetworkHelper.TryGetPlayerSpecData(buffer, out PlayerSpecNetworkData playerSpecData))
        {
            int keeperIndex = PlayerTurnHandler.Index > localIndex ? PlayerTurnHandler.Index - 1 : PlayerTurnHandler.Index;
            _scriptKeepers[keeperIndex].playerCollector.UpdateSpecs(playerSpecData.gold, playerSpecData.health, playerSpecData.goblet);
            return true;
        }
        return false;
    }

    private bool IsPlayerListNetworkData(byte[] buffer)
    {
        if (NetworkHelper.TryGetPlayerListData(buffer, out PlayerListNetworkData playerListData))
        {
            var playerList = NetworkHelper.ByteArrayToSteamId(playerListData.playerList);

            if (playerListData.type == MessageType.UpdatePlayers)
            {
                PlayerTurnHandler.UpdatePlayers(playerList);
            }
            else if (playerListData.type == MessageType.CreatePlayers)
            {
                PlayerTurnHandler.SpawnPlayers(playerList);
            }

            UpdateRemoteScriptKeeper();
            return true;
        }
        return false;
    }

    public void UpdateRemoteScriptKeeper()
    {
        // Players except local player
        _scriptKeepers = new RemoteScriptKeeper[PlayerTurnHandler.PlayerCount - 1];

        // TODO: scriptKeeper'lar index bound olmadýðý için hatalý alýnabilir...
        int i = 0, j = 0;
        foreach (var steamId in PlayerTurnHandler.SteamIds)
        {
            if (steamId != SteamManager.Instance.PlayerSteamId)
                _scriptKeepers[j++] = PlayerTurnHandler.Players[i].GetComponent<RemoteScriptKeeper>();
            else
                localIndex = i;

            i++;
        }
    }
}
