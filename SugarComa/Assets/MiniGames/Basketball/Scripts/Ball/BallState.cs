using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.Ball
{
    public class BallState : MonoBehaviour
    {
        private bool isReady;
        public bool IsReady { get => isReady; set => isReady = value; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BallResetter"))
            {
                isReady = true;
            }
        }
    }
}