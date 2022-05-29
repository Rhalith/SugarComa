using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainRouter : MonoBehaviour
{
    public List<Transform> allObjects;
    [SerializeField] Router router;
    [SerializeField] Movement movement;

    public void createRoute(int x)
    {
        for (int i = 0; i < x; i++)
        {
            router.childObjectList.Add(allObjects[i]);
        }
        movement.currentRouter = router;
    }
}
