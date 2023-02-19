using Assets.MainBoard.Scripts.Player.States;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Route
{
    public class GoalChestAnimation : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private GoalSelector goalSelector;
        [SerializeField] private Animator anim;
        #endregion

        private int count;

        #region Properties
        public GoalSelector GoalSelector { get { return goalSelector; } set { goalSelector = value; } }
        #endregion

        #region Called by ChestAnimation(Torus)
        // Works when game stars and one time. After count become 2, it did not works. 
        public void ChangePlatform()
        {
            if (!goalSelector.isGoalActive)
                goalSelector.ChangeActiveObject(0);

            if (count < 2) 
                count++;
        }

        // Main Chest Creation method. ChangePlatform and this method call inside the ChestOpeningAnimation animation.
        public void ResetCameraPriority()
        {
            string go = gameObject.name;

            goalSelector.ResetGoalCameraPriority();

            // Closes torus
            gameObject.SetActive(false);


            if (count > 1 && !GoalSelector.isAnyGoalPlatform)
            {
                goalSelector.ChangeActiveObject(1);
            }
        }

        public void ResetPlayerAct()
        {
            PlayerStateContext.canPlayersAct = true;
        }
        #endregion

        #region Called by Chest's Animation
        // Starts chest opening animation
        public void StartChestOpeningAnimation()
        {
            string go = gameObject.name;

            Animator animator = GetComponent<Animator>();
            animator.SetBool("isOpened", true);
            goalSelector.isChestAnim = true;
        }

        // Calls inside chest's Chest opening animation.
        public void StartPlatformChangeAnimation()
        {
            string go = gameObject.name;

            GoalSelector.isAnyGoalPlatform = false;
            goalSelector.PlatformChangerObject.SetActive(true);
            goalSelector.PlatformChangerObject.GetComponent<Animator>().SetTrigger("TakeChest");

            goalSelector.isChestAnim = false;
        }
        #endregion
    }
}