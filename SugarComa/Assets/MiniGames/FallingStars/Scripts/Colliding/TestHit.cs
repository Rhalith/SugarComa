using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHit : MonoBehaviour, IHitResponder
{
    [SerializeField] private bool _isAttack;
    [SerializeField] private HitBox _hitbox;
    public int damage => 0;

    public bool CheckHit(HitData data)
    {
        return true;
    }

    public void Response(HitData data)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _hitbox.HitResponder = this;
    }

    void Update()
    {
        _hitbox.CheckHit();
    }
}
