using Assets.MiniGames.FallingStars.Scripts.GameManaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolManagerTester : MonoBehaviour
{
    public PoolManager poolManager;
    public GameObject meteor;
    private GameObject instance;

    [ContextMenu("Get From Pool")]
    private void GetFromPool()
    {
        instance = poolManager.GetFromPool(0);
    }

    [ContextMenu("Add to Pool")]
    private void AddToPool()
    {
        poolManager.AddToPool(instance);
    }
}
