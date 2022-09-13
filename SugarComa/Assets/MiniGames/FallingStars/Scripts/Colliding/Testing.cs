using Assets.MiniGames.FallingStars.Scripts.Colliding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour, IHurtResponder
{
    private List<HurtBox> _hurtBoxes = new();
    public bool CheckHit(HitData data)
    {
        return true;
    }

    public void Response(HitData data)
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        _hurtBoxes = new List<HurtBox>(GetComponentsInChildren<HurtBox>());
        foreach (HurtBox _hurtbox in _hurtBoxes)
        {
            _hurtbox.HurtResponder = this;
        }
    }

}
