using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShadow : MonoBehaviour
{
    public bool isIn;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ColliderChecker")) isIn = true;
    }
}
