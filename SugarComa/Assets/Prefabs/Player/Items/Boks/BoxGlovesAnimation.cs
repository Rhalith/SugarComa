using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGlovesAnimation : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] Animator _animator;
    [SerializeField] BoxGloves _boxGloves;

    private bool isHit = false;
    /// <summary>
    /// 1 for hit, 0 for reset.
    /// </summary>
    /// <param name="i"></param>
    private void HitSet(int i)
    {
        _animator.SetBool("hit", i !=0);
    }

    public void HitAnimation()
    {
        if (!isHit)
        {
            print("first");
            HitSet(1);
            isHit = true;
        }
        else
        {
            print("second");
            HitSet(0);
            isHit = false;
            
        }
    }

    public void BoxGlovesUsed()
    {
        _playerAnimator.SetBool("itemUsing", false);
        _playerAnimator.SetBool("boks", false);
        _boxGloves.TakeGlovesFromPlayer();
    }
    
}
