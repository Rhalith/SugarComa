using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShadow : MonoBehaviour
{
    private bool _isIn;

    public bool IsIn { get => _isIn; set => _isIn = value; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ColliderChecker")) IsIn = true;
    }
}
