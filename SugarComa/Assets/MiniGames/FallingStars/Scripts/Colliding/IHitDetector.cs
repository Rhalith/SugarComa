using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitDetector
{
    public IHitResponder HitResponder { get; set; }
    public void CheckHit();
}
