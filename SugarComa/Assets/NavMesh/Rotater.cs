using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rotater : MonoBehaviour
{
    [SerializeField] NavMeshObstacle obstacle;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] MeshRenderer leftSelecter, rightSelecter;
    [SerializeField] Material greenMaterial, redMaterial;
    [SerializeField] Transform leftDestination, rightDestination;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.isStopped = true;
            playerMovement.steps--;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            obstacle.enabled = true;
            if (Input.GetKeyDown(KeyCode.A))
            {
                leftSelecter.material = greenMaterial;
                rightSelecter.material = redMaterial;
                agent.SetDestination(leftDestination.position);
                playerMovement.target = leftDestination;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                rightSelecter.material = greenMaterial;
                leftSelecter.material = redMaterial;
                agent.SetDestination(rightDestination.position);
                playerMovement.target = rightDestination;
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("Y");
                agent.isStopped = false;
            }
        }
    }

}
