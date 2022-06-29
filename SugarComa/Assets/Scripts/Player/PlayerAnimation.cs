using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public bool _idle;
    public bool _jump;
    public bool _run;
    public bool _land;
    public bool _surprised;

    [SerializeField] Animator _animator;
    [SerializeField] PlayerMovement _playerMovement;

    private void IdleSet()
    {

        _animator.SetBool("idle", !_animator.GetBool("idle"));
        _idle = !_idle;
    }

    private void RunSet()
    {
        _animator.SetBool("running", !_animator.GetBool("running"));
        _run = !_run;
    }

    private void JumpSet()
    {
        _animator.SetBool("jump", !_animator.GetBool("jump"));
        _jump = !_jump;
    }

    private void LandSet()
    {
        _animator.SetBool("landing", !_animator.GetBool("landing"));
        _land = !_land;
    }
    private void SurpriseSet()
    {
        _animator.SetBool("surprised", !_animator.GetBool("surprised"));
        _surprised = !_surprised;
    }

    //Jumping and rolling dice
    public void RollDice()
    {
        IdleSet();
        JumpSet();
    }

    //After jumping start running
    public void StartRunning()
    {
        SetIsRunning("true");
        JumpSet();
        RunSet();
    }

    //If player stops
    public void StopRunning()
    {
        SetIsRunning("false");
        RunSet();
        IdleSet();
    }

    //If player is stopped in Selector and selected
    public void ContinueRunning()
    {
        SetIsRunning("true");
        RunSet();
        IdleSet();
    }

    public void SetIsAnimation()
    {
        _playerMovement.isAnimationStopped = !_playerMovement.isAnimationStopped;
    }

    public void LandPlayer()
    {
        LandSet();
    }

    public void SetIsRunning(string value = "true")
    {
        if (value.Equals("false"))
        {
            _playerMovement.isRunningAnimation = false;
            return;
        }
        else
        {
            _playerMovement.isRunningAnimation = true;
            return;
        }
    }
}
