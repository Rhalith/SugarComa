using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorColliderChecker : MonoBehaviour
{
    private bool _isIn;
    [SerializeField] private Collider _currentChecker;

    public bool IsIn { get => _isIn; set => _isIn = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ColliderChecker") && other != _currentChecker)
        {
            print("yes"+other.name);
            IsIn = true;
        }
    }
}
