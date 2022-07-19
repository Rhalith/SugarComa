using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    struct TempStruct
    {
        public Vector3 direction;
    }
    
    public float movementSpeed = 1f;
    private bool isMoved = false;
    private static bool canMove = false;
    private static Vector3 moveDirection;
    
    
    private SteamManager steamManager;
    TempStruct temp = new TempStruct();

    private void Awake()
    {
        steamManager = GameObject.Find("SteamManager").GetComponent<SteamManager>();
    }

    void Update()
    {
        if (!canMove)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                isMoved = true;
                temp.direction = Vector3.left * movementSpeed;
            }
        
            if (Input.GetKeyDown(KeyCode.D))
            {
                isMoved = true;
                temp.direction = Vector3.right * movementSpeed;
            }
        
            if (Input.GetKeyDown(KeyCode.W))
            {
                isMoved = true;
                temp.direction = Vector3.up * movementSpeed;
            }
        
            if (Input.GetKeyDown(KeyCode.S))
            {
                isMoved = true;
                temp.direction = Vector3.down * movementSpeed;
            }
        }

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
