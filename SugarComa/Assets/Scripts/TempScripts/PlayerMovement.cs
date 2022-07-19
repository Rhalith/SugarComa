using Networking;
using UnityEngine;

namespace TempScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public float movementSpeed = 1f;
        private bool isMoved = false;
        private static bool canMove = false;
        private static Vector3 moveDirection;
    
        private SteamManager steamManager;
        private SteamServerManager serverManager;
        TempStructScript.TempStruct temp;

        private void Awake()
        {
            steamManager = GameObject.Find("SteamManager").GetComponent<SteamManager>();
            serverManager = GameObject.Find("ServerManager").GetComponent<SteamServerManager>();
        }

        private void Start()
        {
            temp.id = steamManager.PlayerSteamId;
        }

        void Update()
        {
            if (!canMove)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    isMoved = true;
                    temp.direction = Vector3.left * movementSpeed;
                    temp.dirStr = "left";
                }
        
                if (Input.GetKeyDown(KeyCode.D))
                {
                    isMoved = true;
                    temp.direction = Vector3.right * movementSpeed;
                    temp.dirStr = "right";
                }
        
                if (Input.GetKeyDown(KeyCode.W))
                {
                    isMoved = true;
                    temp.direction = Vector3.up * movementSpeed;
                    temp.dirStr = "up";
                }
        
                if (Input.GetKeyDown(KeyCode.S))
                {
                    isMoved = true;
                    temp.direction = Vector3.down * movementSpeed;
                    temp.dirStr = "down";
                }
            }

            // Çok fazla çağırılırsa crash verdiriyor...
            // diye düşünüyordum ama buttonların kilitlerini kaldırınca
            // 2k-3k kez sending ve receiving yapmasına rağmen crash vermedi.
            // Başka bir neden var ama henüz bulamadım.
            if (isMoved)
            {
                isMoved = false;
                SendMoveDirection();
            }

            if (canMove)
            {
                transform.Translate(moveDirection);
                canMove = false;
            }
        }

        void SendMoveDirection()
        {
            SteamServerManager.SendingMessages(steamManager.PlayerSteamId, SteamServerManager.Serialize(temp));
        }

        public static void SetDirection(Vector3 direction)
        {
            moveDirection = direction;
            canMove = true;
        }
    }
}
