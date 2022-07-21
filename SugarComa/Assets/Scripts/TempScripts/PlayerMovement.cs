using Networking;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1f;
    private static bool canMove = false;
    private static Vector3 moveDirection;

    private void Start()
    {
        SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
    }

    void Update()
    {
        if (!canMove)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                NetworkData networkData =
                    new NetworkData(MessageType.InputDown, Vector3.left * movementSpeed, Direction.Left);
                SendMoveDirection(networkData);
            }
        
            if (Input.GetKeyDown(KeyCode.D))
            {
                NetworkData networkData =
                    new NetworkData(MessageType.InputDown, Vector3.right * movementSpeed, Direction.Right);
                SendMoveDirection(networkData);
            }
        
            if (Input.GetKeyDown(KeyCode.W))
            {
                NetworkData networkData =
                    new NetworkData(MessageType.InputDown, Vector3.up * movementSpeed, Direction.Up);
                SendMoveDirection(networkData);
            }
        
            if (Input.GetKeyDown(KeyCode.S))
            {
                NetworkData networkData =
                    new NetworkData(MessageType.InputDown, Vector3.down * movementSpeed, Direction.Down);
                SendMoveDirection(networkData);
            }
        }
        else
        {
            transform.Translate(moveDirection);
            canMove = false;
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

        moveDirection = networkData.position;
        canMove = true;
    }
}
