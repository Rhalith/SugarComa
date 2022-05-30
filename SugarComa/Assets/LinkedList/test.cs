using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinkedList
{
public class test : MonoBehaviour
{
    [SerializeField] List<Transform> platforms;
    [SerializeField] List<Mapper> _platforms;
    [Serializable] 
    public class Mapper 
    {
        public Node key;
        public List<Node> values;
    }

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                platforms.Add(gameObject.transform.GetChild(i));
            }
        }
    }

}
}
