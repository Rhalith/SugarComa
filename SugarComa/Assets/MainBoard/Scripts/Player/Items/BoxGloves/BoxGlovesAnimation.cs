using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Items.BoxGloves
{
    public class BoxGlovesAnimation : MonoBehaviour
    {
        [SerializeField] Animator _playerAnimator;
        [SerializeField] Animator _animator;
        [SerializeField] BoxGloves _boxGloves;

        /// <summary>
        /// 1 for hit, 0 for reset.
        /// </summary>
        /// <param name="i"></param>
        private void HitSet(int i)
        {
            _animator.SetBool("hit", i != 0);
        }
        /// <summary>
        /// Triggers the hit animation.
        /// </summary>
        public void HitAnimation()
        {
            HitSet(1);
        }
        /// <summary>
        /// It is using at the end of BoxHit animation.
        /// </summary>
        public void BoxGlovesUsed()
        {
            _playerAnimator.SetBool("itemUsing", false);
            _playerAnimator.SetBool("boks", false);
            HitSet(0);
            _boxGloves.TakeGlovesFromPlayer();
        }

    }
}