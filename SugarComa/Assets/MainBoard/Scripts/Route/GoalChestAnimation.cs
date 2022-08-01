using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalChestAnimation : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] GoalSelector _goalSelector;
    #endregion

    private int count;

    #region Priorities
    public GoalSelector GoalSelector { get { return _goalSelector; } set { _goalSelector = value; } }
    #endregion
    public void ChangePlatform()
    {
        if (!_goalSelector.isGoalActive) _goalSelector.ChangeActiveObject(0);
        if(count < 2) count++;
    }

    public void ResetCameraPriority()
    {
        _goalSelector.ResetGoalCameraPriority();
        gameObject.SetActive(false);
        if (count > 1 && !GoalSelector.isAnyGoalPlatform)
        {
            _goalSelector.ChangeActiveObject(1);
        }
    }

    public void StartChestOpeningAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isOpened", true);
    }

    public void StartPlatformChangeAnimation()
    {
        GoalSelector.isAnyGoalPlatform = false;
        _goalSelector._platformChangerObject.SetActive(true);
    }


}
