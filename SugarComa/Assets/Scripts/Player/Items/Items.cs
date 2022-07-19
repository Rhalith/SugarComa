using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItems
{
    public void UseItem();
}

public interface IDamageItems : IItems
{
    /// <summary>
    /// Decreases player health.
    /// </summary>
    /// <param name="playerCollector"></param>
    public void DamageHealth(PlayerCollector playerCollector);
}

public interface IHealerItems : IItems
{
    /// <summary>
    /// Increases player health.
    /// </summary>
    /// <param name="playerCollector"></param>
    public void HealerHealth(PlayerCollector playerCollector);
}
