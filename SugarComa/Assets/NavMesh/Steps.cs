using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Steps : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] NavMeshObstacle obstacle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.steps--;
            obstacle.enabled = true;
            if (playerMovement.steps == 0)
            {
                agent.isStopped = true;
            }
        }
    }
}
