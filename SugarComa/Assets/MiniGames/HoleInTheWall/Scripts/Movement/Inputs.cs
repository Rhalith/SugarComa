using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Movement
{
    public class Inputs : MonoBehaviour
    {
        private PlayerInputs _playerInputs;

        private Vector2 _movement;

        public Vector2 Movement { get => _movement; }

        // Start is called before the first frame update
        void Start()
        {
            InitializeInputs();
        }



        // Update is called once per frame
        void Update()
        {

        }

        private void InitializeInputs()
        {
            _playerInputs = new PlayerInputs();
            _playerInputs.Movement.Move.started += Move_started;
            _playerInputs.Movement.Move.performed += Move_performed;
            _playerInputs.Movement.Move.canceled += Move_canceled;
            
           
        }
        
        public void OnMove(InputAction.CallbackContext obj)
        {
            Debug.Log("TEST");
            _movement = obj.ReadValue<Vector2>();
            Debug.Log("TEST");
            Debug.Log(_movement.normalized);
        }
        private void Move_started(InputAction.CallbackContext obj)
        {
            _movement = obj.ReadValue<Vector2>();
            Debug.Log(_movement.normalized);
        }
        private void Move_canceled(InputAction.CallbackContext obj)
        {
            _movement = obj.ReadValue<Vector2>();
            Debug.Log(_movement.normalized);
        }
        private void Move_performed(InputAction.CallbackContext obj)
        {
            _movement = obj.ReadValue<Vector2>();
            Debug.Log(_movement.normalized);
        }
    }
}