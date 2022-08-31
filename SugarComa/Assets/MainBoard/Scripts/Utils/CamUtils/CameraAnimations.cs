using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.UI;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Utils.CamUtils
{
    //TODO
    public class CameraAnimations : MonoBehaviour
    {
        GobletSelection _gobletSelection;
        [SerializeField] GoalSelector _goalSelector;
        //public void SetGobletSelection(GobletSelection gobletSelection)
        //{
        //    _gobletSelection = gobletSelection;
        //}
        public void AfterGoalSelector()
        {
            _goalSelector.ResetGoalCameraPriority();
        }
    }
}