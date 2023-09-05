using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Player.Handlers;
using Assets.MainBoard.Scripts.Player.States;
using Assets.MainBoard.Scripts.UI;
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
        }

        // Main Chest Creation method. ChangePlatform and this method call inside the ChestOpeningAnimation animation.
        public void ResetCameraPriority()
        {
            goalSelector.ResetGoalCameraPriority();

            // Closes torus
            gameObject.SetActive(false);


            if (!GoalSelector.isAnyGoalPlatform)
            {
                goalSelector.ChangeActiveObject(1);
            }
        }

        public void ResetPlayerAct()
        {
            // Change... After chest animations over and if it is players turn. Its turns overs
            // We checking it from PlayerHandler. Change it.
            if (PlayerHandler.Instance.IsChestTaken)
            {
                PlayerHandler.Instance.IsChestTaken = false;
                RemoteMessageHandler.Instance.SendTurnOver(PlayerStateContext.Instance);
            }

            GoalSelector.isAnyGoalPlatform = true;
            PlayerStateContext.canPlayersAct = true;
        }
        #endregion
    }
}