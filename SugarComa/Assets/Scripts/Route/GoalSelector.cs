using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSelector : MonoBehaviour
{
    [SerializeField] List<Platform> platforms;

    [SerializeField] PlayerMovement _playerMovement;
    
    public void RandomGoalSelect()
    {
        int i = Random.Range(0, platforms.Count+1);
        if (platforms[i].specification != PlatformSpecification.Goal)
        {
            platforms[i].specification = PlatformSpecification.Goal;
            print(platforms[i]);
            return;
        }
        else
        {
            RandomGoalSelect();
        }
    }
}
