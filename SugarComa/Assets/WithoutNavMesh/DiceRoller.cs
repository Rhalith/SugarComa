using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] int roll;
    [SerializeField] mainRouter router;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !movement.isMoving && !movement.isSelecting)
        {
            movement.steps = 5;
            Debug.Log("DiceRolled " + movement.steps);

            movement.startMove();
        }
        else if (Input.GetKeyDown(KeyCode.P) && !movement.isMoving && !movement.isSelecting)
        {
            rollDice(roll);
            movement.startMove();
        }
        else if (Input.GetKeyDown(KeyCode.X) && !movement.isMoving && !movement.isSelecting)
        {
            goXstep(roll);
            movement.startMove();
        }
    }

    void rollDice(int num)
    {
        movement.steps += num;
    }

    void goXstep(int step)
    {
        movement.steps += step;
        movement.overrideSelection = true;
        router.createRoute(step);
    }
}
