using UnityEngine;

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
