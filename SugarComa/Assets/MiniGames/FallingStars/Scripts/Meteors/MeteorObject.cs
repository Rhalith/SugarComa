using System.Collections;
using System.Collections.Generic;
using Assets.MiniGames.FallingStars.Scripts.Meteors;
using Assets.MiniGames.FallingStars.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorObject : MonoBehaviour
{
    public delegate void MeteorAction();

    public MeteorAction OnMeteorHit;

    private void Awake()
    {
        OnMeteorHit += DisableObject;
    }
    void OnTriggerEnter(Collider collider)
    {
        CheckHit(collider);
    }

    public void OnHit()
    {
        OnMeteorHit?.Invoke();
    }
    private void CheckHit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerSpecs>().KillPlayer();
        }
    }

    /// <summary>
    /// Invokes at the end of MeteorAnimation
    /// </summary>
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }

}
