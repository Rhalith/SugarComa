using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtBox
{
    public bool Active { get; }
    public GameObject Owner { get; }
    public Transform Transform { get; }
    public HurtBoxType Type { get; }
    public IHurtResponder HurtResponder { get; set; }
    public bool CheckHit(HitData data);
}
