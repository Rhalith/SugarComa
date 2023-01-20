using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Player.States;
using Assets.MainBoard.Scripts.Player.Handlers;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Networking.LobbyNetworking;
using Assets.MainBoard.Scripts.Networking.MainBoardNetworking;

public class RemoteMessageHandler : MonoBehaviour
{
    private static RemoteMessageHandler _instance;
    public static RemoteMessageHandler Instance => _instance;


    public PlayerHandler playerHandler;
    public GoalSelector goalSelector;
    private RemoteScriptKeeper[] _scriptKeepers;
    public int localIndex;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(this);
            return;
        }

        _instance = this;

        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    private void OnDestroy()
    {
        SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
    }

    #region Receivers
    private void OnMessageReceived(SteamId steamId, byte[] buffer)
    {
        Debug.Log(PlayerTurnHandler.Index);

        if (IsNetworkData(steamId, buffer)) return;
        if (IsAnimationStateData(buffer)) return;
        if (IsPlayerSpecNetworkData(buffer)) return;
        if (IsPlayerListNetworkData(buffer)) return;
        if (IsTurnOverNetworkData(buffer)) return;
        if (IsMiniGameNetworkData(buffer)) return;
        if (IsChestNetworkData(buffer)) return;
    }

    private bool IsNetworkData(SteamId steamId, byte[] buffer)
    {
        // TODO: NetworkData interface'i default deðer döndürüyor hatalý olarak, bu interface'i ayýrýnca düzelmiþti...
        if (MBNetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
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
        if (MBNetworkHelper.TryGetAnimationData(buffer, out AnimationStateData animationStateData))
        {
            int keeperIndex = PlayerTurnHandler.Index > localIndex ? PlayerTurnHandler.Index - 1 : PlayerTurnHandler.Index;
            _scriptKeepers[keeperIndex].playerAnimation.UpdateAnimState(animationStateData.animBoolHash);
            return true;
        }
        return false;
    }

    private bool IsPlayerSpecNetworkData(byte[] buffer)
    {
        if (MBNetworkHelper.TryGetPlayerSpecData(buffer, out PlayerSpecNetworkData playerSpecData))
        {
            int keeperIndex = PlayerTurnHandler.Index > localIndex ? PlayerTurnHandler.Index - 1 : PlayerTurnHandler.Index;
            _scriptKeepers[keeperIndex].playerCollector.UpdateSpecs(playerSpecData.gold, playerSpecData.health, playerSpecData.goblet);
            return true;
        }
        return false;
    }
    
    private bool IsTurnOverNetworkData(byte[] buffer)
    {
        if (MBNetworkHelper.TryGetTurnNetworkData(buffer, out TurnNetworkData turnData))
        {
            PlayerTurnHandler.NextPlayer();
            playerHandler.ChangeCurrentPlayer(PlayerTurnHandler.Index);

            return true;
        }
        return false;
    }
    
    private bool IsMiniGameNetworkData(byte[] buffer)
    {
        if (MBNetworkHelper.TryGetMiniGameNetworkData(buffer, out MiniGameNetworkData minigameData))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return true;
        }
        return false;
    }

    private bool IsPlayerListNetworkData(byte[] buffer)
    {
        if (MBNetworkHelper.TryGetPlayerListData(buffer, out PlayerListNetworkData playerListData))
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

            var camera = playerHandler.GetCinemachineVirtualCamera(0);
            camera.Priority = 2;

            return true;
        }
        return false;
    }

    private bool IsChestNetworkData(byte[] buffer)
    {
        if (MBNetworkHelper.TryGetChestData(buffer, out ChestNetworkData chestData))
        {
            goalSelector.CreateGoal(chestData.index);

            return true;
        }
        return false;
    }
    #endregion

    #region Senders
    public void SendTurnOver(PlayerStateContext context)
    {
        // Inside IsMyTurn ChangeCurrentPlayer called, if it is false.
        context.IsMyTurn = false;
        PlayerTurnHandler.NextPlayer();
        SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new TurnNetworkData(MessageType.TurnOver)));
    }

    public bool SendNewChestIndex(int index)
    {
        //return SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new ChestNetworkData((byte)index)));
        return SteamServerManager.Instance.SendingMessage(SteamManager.Instance.PlayerSteamId, NetworkHelper.Serialize(new ChestNetworkData((byte)index)));
    }

    #endregion
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
