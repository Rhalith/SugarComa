using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _pathTracker.OnCurrentPlatformChanged += OnCurrentPlatformChanged;
    }
    private void OnDestroy()
    {
        _pathTracker.OnCurrentPlatformChanged -= OnCurrentPlatformChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCurrentPlatformChanged()
    {
        var current = _pathTracker.CurrentPlatform;
        if (current != null && _pathTracker.Next != null)
        {
            NetworkData networkData =
                new NetworkData(MessageType.InputDown, _pathTracker.Next.position);

            SendMoveDirection(networkData);
            _currentPlatform = current;
            _currentStep -= 1;
        }
    }

    private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
    {
        if (!NetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
            return;

        if (networkData.type == MessageType.InputDown)
        {
            _t = 0;
            _startPosition = transform.position;
            _currentPosition = networkData.position;
        }
    }

    void SendMoveDirection(in NetworkData networkData)
    {
        SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(networkData));
    }
}
