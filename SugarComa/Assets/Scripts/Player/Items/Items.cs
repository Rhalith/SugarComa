using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItems
{
    public void UseItem();
}

public interface IDamageItems : IItems
{
    public void DamageHealth(PlayerCollector playerCollector);
}

public interface IHealerItems : IItems
{
    public void HealerHealth(PlayerCollector playerCollector);
}
