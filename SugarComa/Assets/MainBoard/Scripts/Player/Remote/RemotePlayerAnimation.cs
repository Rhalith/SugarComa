using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.Items.BoxGloves;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using Steamworks;
using System.Collections;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Remote
{
    public class RemotePlayerAnimation : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private GameObject _boxGloves;
        [SerializeField] private Animator anim;
        #endregion

        #region Private Field
        private AnimatorControllerParameter[] animControlParams;
        #endregion

        void OnEnable()
        {
            // Gets all animator parameter as AnimatorControllerParameter array
            animControlParams = anim.parameters;
        }

        private void Awake()
        {
            SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
        }

        private void OnDisable()
        {
            SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
        }

        private void OnMessageReceived(SteamId steamid, byte[] buffer)
        {
            if (NetworkHelper.TryGetAnimationData(buffer, out AnimationStateData animationStateData))
            {
                UpdateAnimState(animationStateData.prevAnimBoolHash, animationStateData.nextAnimBoolHash);
            }
        }

        private void UpdateAnimState(int prevAnimBoolHash, int nextAnimBoolHash)
        {
            foreach (AnimatorControllerParameter param in animControlParams)
            {
                int animBoolHash = Animator.StringToHash(param.name);

                if (animBoolHash == nextAnimBoolHash)
                {
                    anim.SetBool(param.name, true);
                }
                else if (animBoolHash == prevAnimBoolHash)
                {
                    anim.SetBool(param.name, false);
                }
            }
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