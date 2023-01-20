using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.UI;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Utils.CamUtils
{
    //TODO
    public class CameraAnimations : MonoBehaviour
    {
        [SerializeField] GoalSelector _goalSelector;

        public void AfterGoalSelector()
        {
            _goalSelector.ResetGoalCameraPriority();
        }
    }
}