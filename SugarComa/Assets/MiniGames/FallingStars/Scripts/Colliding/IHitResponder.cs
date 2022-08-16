using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitResponder
{
    int damage { get; }
    public bool CheckHit(HitData data);
    public void Response(HitData data);
}
