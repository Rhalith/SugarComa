using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public bool IsRunning;

    private void RunSet(int i)
    {
        IsRunning = i != 0;
        _animator.SetBool("isRunning", i != 0);
    }

    public void StartRunning()
    {
        RunSet(1);
    }
    public void StopRunning()
    {
        RunSet(0);
    }
}
