using Assets.MainBoard.Scripts.Player.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public bool IsRunning;

    public Animator Animator { get => _animator;}

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

    public void ChangeAnimatorSpeed(float speed)
    {
        switch (speed)
        {
            case float i when i > 0f && i < 30f:
                _animator.speed = 0.4f;
                break;
            case float i when i >= 30f && i < 65f:
                _animator.speed = 0.5f;
                break;
            case float i when i >= 65f && i < 100f:
                _animator.speed = 0.8f;
                break;
            case float i when i >= 100f && i < 125f:
                _animator.speed = 1f;
                break;
            case float i when i >= 125f && i < 150f:
                _animator.speed = 1.2f;
                break;
            case float i when i >= 150f && i < 210f:
                _animator.speed = 1.4f;
                break;
            default:
                _animator.speed = 1f;
                break;
        }
    }
}
