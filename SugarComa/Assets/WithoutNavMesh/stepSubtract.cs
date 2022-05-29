using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepSubtract : MonoBehaviour
{
    [SerializeField] Movement movement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && movement.isMoving)
        {
            movement.steps--;
        }
    }
}
