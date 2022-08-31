using Assets.MainBoard.Scripts.Utils.InventorySystem;
using Assets.MainBoard.Scripts.Player.Movement;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Items.BoxGloves
{
    public class BoxGloves : MonoBehaviour, IDamageItems
    {
        #region SerializeFields
        [SerializeField] ItemPool _itemPool;
        [SerializeField] ItemObject _itemObject;
        [SerializeField] GameObject _player;
        [SerializeField] int damage;
        [SerializeField] BoxGlovesAnimation _boxGlovesAnimation;
        [SerializeField] Animator _playerAnimator;
        #endregion

        public List<PlayerCollector> otherPlayersCollector;

        public GameObject _hitBox;

        public bool isHitPlayer;

        public void DamageHealth(PlayerCollector playerCollector)
        {
            if (playerCollector != null)
            {
                playerCollector.DamagePlayer(damage);
                
                // TODO: Send Message ile hasar yiyen player'ýn id'si ve hasarý yolla.
                // DamagePlayer'ý ve KillPlayer'ý (PlayerCollector içindeler) IdleState'e taþýyabiliriz. 
            }
        }

        /// <summary>
        /// Use box gloves.
        /// </summary>
        public void UseItem()
        {
            _hitBox.SetActive(false);
            _boxGlovesAnimation.HitAnimation();
        }
        /// <summary>
        /// Triggers to player taking box gloves animation. Using on boks eldiveni button in User Interface gameObject.
        /// </summary>
        public void TakeGlovesToPlayer()
        {
            ItemUsing.BoxGlovesUsing = true;
            _playerAnimator.SetBool("boxing", true);
        }

        /// <summary>
        /// If player is in hitbox damage it and removes item from inventory. 
        /// </summary>
        public void TakeGlovesFromPlayer()
        {
            if (isHitPlayer)
            {
                foreach (var playerCollector in otherPlayersCollector)
                {
                    DamageHealth(playerCollector);
                }
            }
            otherPlayersCollector.Clear();
            _itemObject.RemoveItem();
            // TODO:
            //_playerMovement.GameController.ChangeInventory();
            _itemPool.CloseItem();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject != _player)
            {
                isHitPlayer = true; 
                otherPlayersCollector.Add(other.GetComponent<PlayerCollector>());
            }
        }

        // TODO: Hasar verilmeden player silinebilir...
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other != _player)
            {
                isHitPlayer = false; 
                otherPlayersCollector.Remove(other.GetComponent<PlayerCollector>());
            }
        }
    }
}