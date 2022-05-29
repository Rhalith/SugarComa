using System.Collections;
using System.Collections.Generic;
using UnityEngine; using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public int steps;
    [SerializeField] NavMeshRouter router;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && agent.isStopped)
        {
            steps = 5;
            Debug.Log("DiceRolled " + steps);
            router.bakeRoute(steps);
            agent.SetDestination(target.position);
            agent.isStopped = false;
        }
        if (steps <= 0)
        {
            agent.isStopped = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            agent.isStopped = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            agent.isStopped = false;
        }
    }
}
