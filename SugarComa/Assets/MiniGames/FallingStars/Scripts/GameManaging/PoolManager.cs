using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.GameManaging
{
    public class PoolManager : MonoBehaviour
    {
        [System.Serializable]
        public struct Pool
        {
            public List<GameObject> pooledObjects;
        }

        [SerializeField] private Pool[] _pools;

        public Pool[] Pools { get => _pools; private set => _pools = value; }

        public void AddToPool(GameObject obj)
        {
            if (obj.activeInHierarchy) obj.SetActive(false);
        }

        public void AddToPool(GameObject[] objs)
        {
            foreach(GameObject obj in objs)
            {
                if (obj.activeInHierarchy) obj.SetActive(false);
            }
        }

        public GameObject GetFromPool(int poolIndex)
        {
            GameObject gameObject = null;
            foreach (GameObject obj in Pools[poolIndex].pooledObjects)
            {
                if (!obj.activeInHierarchy && gameObject == null)
                {
                    gameObject = obj;
                    obj.SetActive(true);
                }
            }
            if (gameObject == null) print("pool yetersiz!!!!");
            return gameObject;
        }

        public GameObject[] GetFromPool(int poolIndex, int count)
        {
            GameObject[] gameObjects = null;
            foreach (GameObject obj in Pools[poolIndex].pooledObjects)
            {
                if (!obj.activeInHierarchy && gameObjects.Length < count)
                {
                    gameObjects[gameObjects.Length] = obj;
                    obj.SetActive(true);
                }
            }
            if (gameObjects.Length < count) print("pool yetersiz!!!!");
            return gameObjects;
        }
    }
}