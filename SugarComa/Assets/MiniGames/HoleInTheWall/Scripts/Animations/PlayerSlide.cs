using Assets.MiniGames.HoleInTheWall.Scripts.Player.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Animations
{
    public class PlayerSlide : MonoBehaviour
    {
        #region SerializeField
        [Header("Other Scripts")]
        [SerializeField] private PlayerMovement _playerMovement;

        public void Slide(bool start)
        {
            if (start)
            {
                _playerMovement.CapsuleCollider.height = 2f;
                _playerMovement.CapsuleCollider.center = new Vector3(0, 0f, 0);
                _playerMovement.IsCrouched = true;
            }
            else
            {
                _playerMovement.CapsuleCollider.height = 4f;
                _playerMovement.CapsuleCollider.center = new Vector3(0, 1f, 0);
                _playerMovement.IsCrouched = false;
            }
        }
        #endregion
    }
}