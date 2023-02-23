using UnityEngine;

namespace Assets.MainBoard.Scripts.Route
{
    public class GoalChestAnimation : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private GoalSelector goalSelector;
        #endregion

        #region Properties
        public GoalSelector GoalSelector { get { return goalSelector; } set { goalSelector = value; } }
        #endregion

        #region Called by Chest's Animation
        public void StartChestOpeningAnimation()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("isOpened", true);
            goalSelector.isChestAnim = true;
        }

        // Calls inside chest's Chest opening animation.
        public void StartPlatformChangeAnimation()
        {
            GoalSelector.isAnyGoalPlatform = false;
            goalSelector.PlatformChangerObject.SetActive(true);
            goalSelector.PlatformChangerObject.GetComponent<Animator>().SetTrigger("TakeChest");

            goalSelector.isChestAnim = false;
        }
        #endregion
    }
}