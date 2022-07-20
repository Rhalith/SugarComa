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
        PlayerInfo.Info playerInfo;

        [SerializeField] private GameManager gameManager;
        [SerializeField] private SteamManager steamManager;
        [SerializeField] private SteamServerManager serverManager;

        private void Awake()
        {
            gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        private void Start()
        {
            steamManager = gameManager.steamManager;
            serverManager = gameManager.serverManager;
            playerInfo.id = steamManager.PlayerSteamId;
        }

        void Update()
        {
            if (!canMove)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    isMoved = true;
                    playerInfo.direction = Vector3.left * movementSpeed;
                    playerInfo.dirStr = "left";
                }
        
                if (Input.GetKeyDown(KeyCode.D))
                {
                    isMoved = true;
                    playerInfo.direction = Vector3.right * movementSpeed;
                    playerInfo.dirStr = "right";
                }
        
                if (Input.GetKeyDown(KeyCode.W))
                {
                    isMoved = true;
                    playerInfo.direction = Vector3.up * movementSpeed;
                    playerInfo.dirStr = "up";
                }
        
                if (Input.GetKeyDown(KeyCode.S))
                {
                    isMoved = true;
                    playerInfo.direction = Vector3.down * movementSpeed;
                    playerInfo.dirStr = "down";
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
            SteamServerManager.SendingMessages(steamManager.PlayerSteamId, SteamServerManager.Serialize(playerInfo));
        }

        public static void SetDirection(Vector3 direction)
        {
            moveDirection = direction;
            canMove = true;
        }
    }
}
