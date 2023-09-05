using UnityEngine;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using Assets.MainBoard.Scripts.Player.Items.BoxGloves;

namespace Assets.MainBoard.Scripts.Player.Remote
{
    public class RemotePlayerAnimation : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private RemoteScriptKeeper scKeeper;
        [SerializeField] private GameObject _boxGloves;
        [SerializeField] private Animator anim;
        #endregion

        #region Private Field
        private int _lastAnimHash = -1;
        #endregion

        public void UpdateAnimState(int animBoolHash)
        {
            anim.SetBool(animBoolHash, true);

            if (_lastAnimHash != -1 && _lastAnimHash != animBoolHash)
                anim.SetBool(_lastAnimHash, false);

            _lastAnimHash = animBoolHash;
        }

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