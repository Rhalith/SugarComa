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
            [HideInInspector]
            public Transform parent;

            public GameObject prefab;
            public List<GameObject> pooledObjects;
        }
        [SerializeField] private GameObject _parent;
        [SerializeField] private Pool[] _pools = null;

        public int InitializePool(GameObject prefab, int initPoolSize)
        {
            Pool pool = new Pool();
            pool.prefab = prefab;
            pool.parent = InstantiatePrefab(_parent,transform).transform;
            pool.parent.gameObject.name = pool.prefab.name;
            for(int i =0; i < initPoolSize; i++)
            {
                GameObject obj = InstantiatePrefab(prefab,pool.parent);
                obj.SetActive(false);
                pool.pooledObjects.Add(obj);
            }
            _pools[_pools.Length] = pool;
            return _pools.Length-1;
        }

        public void AddToPoolDoesntExists(int poolIndex, GameObject obj)
        {
                obj.SetActive(false);
                _pools[poolIndex].pooledObjects.Add(obj);
        }

        public void AddToPool(GameObject obj)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
        }

        public GameObject GetFromPool(int poolIndex)
        {
            GameObject gameObject = null;
            foreach(GameObject obj in _pools[poolIndex].pooledObjects)
            {
                if (!obj.activeInHierarchy && gameObject == null)
                {
                    obj.SetActive(true);
                    gameObject = obj;
                }
            }
            if(gameObject == null)
            {
                gameObject = InstantiatePrefab(_pools[poolIndex].prefab, _pools[poolIndex].parent);
                AddToPoolDoesntExists(poolIndex, gameObject);
            }
            return gameObject;
        }

        public GameObject GetFromPool(int poolIndex, int count)
        {
            GameObject[] gameObjects = null;
            foreach (GameObject obj in _pools[poolIndex].pooledObjects)
            {
                if (!obj.activeInHierarchy && gameObjects.Length<count)
                {
                    obj.SetActive(true);
                    gameObjects[gameObjects.Length] = obj;
                    
                }
            }
            if (gameObjects.Length < count)
            {
                for(int i = 0; i< count-gameObjects.Length; i++)
                {
                   GameObject obj = InstantiatePrefab(_pools[poolIndex].prefab, _pools[poolIndex].parent);
                   AddToPoolDoesntExists(poolIndex, obj);
                }
            }
            return gameObject;
        }


        private GameObject InstantiatePrefab(GameObject prefab, Transform parent)
        {
            GameObject obj = Instantiate(prefab, parent);
            return obj;
        }

        private GameObject InstantiatePrefabWithPos(GameObject prefab,Vector3 pos, Transform parent)
        {
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, parent);
            return obj;
        }
    }
}