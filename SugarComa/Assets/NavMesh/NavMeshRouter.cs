using System.Collections;
using System.Collections.Generic;
using UnityEngine; using UnityEngine.AI;

public class NavMeshRouter : MonoBehaviour
{
    public List<NavMeshObstacle> roads;
    public int j;

    public void bakeRoute(int x)
    {
        for (j = 0; j <= x; j++)
        {
            roads[j].enabled = false;
        }
    }
}
