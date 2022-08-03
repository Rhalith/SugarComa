using Assets.MiniGames.FallingStars.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeteorHit
{
    public void KillPlayer(PlayerSpecs player);
    public void DamagePlayer(PlayerSpecs player);
    public bool IsPlayerIn();
}
