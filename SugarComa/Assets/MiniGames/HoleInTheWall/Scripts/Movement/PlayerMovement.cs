using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerInputs playerInputs;
        public void OnMove(InputAction.CallbackContext obj)
        {
            print("zort");
        }

        // Start is called before the first frame update
        void Start()
        {
            playerInputs = new PlayerInputs();
            playerInputs.Movement.Jump.performed += Move_performed;
        }

        private void Move_performed(InputAction.CallbackContext obj)
        {
            print(obj);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}