using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimations : MonoBehaviour
{
    GobletSelection _gobletSelection;
    [SerializeField] GoalSelector _goalSelector;
    public void SetGobletSelection(GobletSelection gobletSelection)
    {
        _gobletSelection = gobletSelection;
    }
    public void AfterGoalSelector()
    {
        _gobletSelection.ContinueToMove();
        _goalSelector.ResetGoalCameraPriority();
    }
    
}
