using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtResponder
{
    public bool CheckHit(HitData data);
    public void Response(HitData data);
}
