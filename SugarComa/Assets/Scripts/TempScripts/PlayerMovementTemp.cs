using Networking;
using UnityEngine;

public class PlayerMovementTemp : MonoBehaviour
{
    public float movementSpeed = 1f;

    private void Start()
    {
        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    private void OnDestroy()
    {
        SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
    }

    void Update()
    {
        Vector3 position = transform.position;
        if (Input.GetKeyDown(KeyCode.A))
        {
            position += Vector3.left * movementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            position += Vector3.right * movementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            position += Vector3.up * movementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            position += Vector3.down * movementSpeed;
        }

        if (transform.position != position)
        {
            NetworkData networkData =
                new NetworkData(MessageType.InputDown, position);
            SendMoveDirection(networkData);
            transform.position = position;
        }
    }

    void SendMoveDirection(in NetworkData networkData)
    {
        // Struct iletirken crash veriyor...
        SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(networkData));
    }

    private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
    {
        if (!NetworkHelper.TryGetNetworkData(buffer, out NetworkData networkData))
            return;

        if (networkData.type == MessageType.InputDown)
        {
            NetworkManager.Instance.playerList[steamid].transform.position = networkData.position;
        }
    }
}
