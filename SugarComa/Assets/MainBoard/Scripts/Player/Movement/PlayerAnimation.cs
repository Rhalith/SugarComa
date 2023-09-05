using Assets.MainBoard.Scripts.Player.Items.BoxGloves;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Movement
{
    [System.Obsolete("Bu script düzenlenip, kaldýrýlacak.")]
    public class PlayerAnimation : MonoBehaviour
    {
        #region SerializeField
        [Header("Other Scripts")]
        [SerializeField] private GameObject _boxGloves;
        #endregion

        /// <summary>
        /// Set box gloves off, Invokes at the start of "BoxGlovesLeaving" animation.
        /// </summary>
        public void SetGlovesOff()
        {
            _boxGloves.SetActive(false);
            ItemUsing.BoxGlovesUsing = false;
            _boxGloves.GetComponent<BoxGloves>()._hitBox.SetActive(false);
        }
        /// <summary>
        /// Set box gloves on, Invokes at the end of "BoxGlovesTaking" animation.
        /// </summary>
        public void SetGlovesOn()
        {
            _boxGloves.SetActive(true);
            ItemUsing.BoxGlovesUsing = true;
            _boxGloves.GetComponent<BoxGloves>()._hitBox.SetActive(true);
        }
    }
}