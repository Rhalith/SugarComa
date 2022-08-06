using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public List<GameObject> pooledObjects;
    }

    [SerializeField] private Pool[] pools = null;
}
