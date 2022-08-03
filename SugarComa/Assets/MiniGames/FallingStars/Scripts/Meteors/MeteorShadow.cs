using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShadow : MonoBehaviour
{
    public bool isPlayerInShadow;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInShadow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInShadow = false;
        }
    }
}
