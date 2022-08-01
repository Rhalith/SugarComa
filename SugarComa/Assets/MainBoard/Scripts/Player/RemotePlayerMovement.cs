using Assets.MainBoard.Scripts.Networking.Utils;
using Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerMovement : MonoBehaviour
{
    [SerializeField] PathTracker _pathTracker;

    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private float _t;

    // çalışmıyolar şuan
    private Platform _currentPlatform;
    private int _currentStep;

    private void Start()
    {
        _pathTracker.OnCurrentPlatformChanged += OnCurrentPlatformChanged;
        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;

    }
    private void OnDestroy()
    {
        _pathTracker.OnCurrentPlatformChanged -= OnCurrentPlatformChanged;
        SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
    }

    void Update()
    {
        if (_currentPosition != _pathTracker.Next.position)
            _currentPosition = _pathTracker.Next.position;

        if (transform.position != _currentPosition)
        {
            _t += Time.deltaTime * _pathTracker.speed;
            // Smooth tracking
            // if object position not equal the current platform position move to position.
            transform.position = Vector3.Lerp(_startPosition, _currentPosition, _t);

            // rotation
            Vector3 movementDirection = (_currentPosition - transform.position).normalized;
            if (movementDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _pathTracker.rotationSpeed * Time.deltaTime);
            }
        }
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
            Debug.Log("Ey");
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