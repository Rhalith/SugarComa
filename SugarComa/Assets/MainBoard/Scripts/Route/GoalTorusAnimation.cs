using Assets.MainBoard.Scripts.Player.States;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Route
{
    public class GoalTorusAnimation : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private GoalSelector goalSelector;
        #endregion

        private int count;

        #region Properties
        public GoalSelector GoalSelector { get { return goalSelector; } set { goalSelector = value; } }
        #endregion

        #region Called by ChestAnimation(Torus)
        // Works when game stars and works only once. After count become 2, it did not works. 
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
    }
}