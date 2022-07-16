using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region SerializeField

    [Header("Animation Flags")]
    [SerializeField] private bool _jump;
    [SerializeField] private bool _run;
    [SerializeField] private bool _land;
    [SerializeField] private bool _surprised;
    [SerializeField] private bool _dead;

    [Header("Other Scripts")]
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private ScriptKeeper _scriptKeeper;
    [SerializeField] private GameObject _boxGloves;
    [SerializeField] private GoalSelector _goalSelector;
    #endregion

    #region Properties
    [SerializeField] GameObject _dice;

    public bool IsLanding => _land;
    public bool IsJumping => _jump;
    public bool IsRunning => _run;
    public bool IsSurprised => _surprised;
    public bool IsDead => _dead;
    public bool IsIdle => !_run && !_land && !_jump && !_surprised && !_dead;

    public GoalSelector GoalSelector { set => _goalSelector = value; }
    #endregion

    private void RunSet(int running)
    {
        _animator.SetBool("running", running != 0);
        _run = running != 0;
    }

    private void JumpSet(int jump)
    {
        _animator.SetBool("jump", jump != 0);
        _jump = jump != 0;
        if(jump == 0)
        {
            _dice.SetActive(false);
            _playerMovement.DiceText.enabled = true;
            IEnumerator waitForCloseText()
            {
                yield return null;
                yield return new WaitForSeconds(0.5f);
                _playerMovement.DiceText.enabled = false;
            }
            StartCoroutine(waitForCloseText());
        }
        
    }

    private void LandSet(int landing)
    {
        _animator.SetBool("landing", landing != 0);
        _land = landing != 0;
        if(_playerMovement.PlayerCollector.isDead && landing == 0)
        {
            DeathSet(0);
            _playerMovement.PlayerCollector.isDead = false;
            _scriptKeeper._playerCamera.Priority = 1;
        }
        if (!GoalSelector.isAnyGoalPlatform && landing == 0)
        {
            _goalSelector.SelectGoalOnStart();
            _dice.SetActive(true);
        }
    }
    private void SurpriseSet(int surprised)
    {
        _animator.SetBool("surprised", surprised != 0);
        _surprised = surprised != 0;
    }
    private void DeathSet(int dying)
    {
        _animator.SetBool("dead", dying != 0);
        _dead = dying != 0;
    }

    //Jumping and rolling dice
    public void RollDice()
    {
        JumpSet(1);
    }

    //After jumping start running
    public void StartRunning()
    {
        RunSet(1);
    }

    //If player stops
    public void StopRunning()
    {
        RunSet(0);
    }

    //If player is stopped in Selector and selected
    public void ContinueRunning()
    {
        RunSet(1);
    }

    public void LandPlayer()
    {
        LandSet(1);
    }
    public void StartDeath()
    {
        DeathSet(1);
    }
    public void AfterDeath()
    {
        _playerMovement.OnDeath();
    }


    public void SetGlovesOff()
    {
        _boxGloves.SetActive(false);
        ItemUsing.BoxGlovesUsing = false;
        _boxGloves.GetComponent<BoxGloves>()._hitBox.SetActive(false);
    }

    public void SetGlovesOn()
    {
        _boxGloves.SetActive(true);
        ItemUsing.BoxGlovesUsing = true;
        _boxGloves.GetComponent<BoxGloves>()._hitBox.SetActive(true);
    }
}
