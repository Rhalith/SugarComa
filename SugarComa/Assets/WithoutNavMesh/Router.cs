using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{
    public List<Transform> childObjectList;
    public Transform lastObject;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < childObjectList.Count; i++)
        {
            Vector3 currentPos = childObjectList[i].position;
            if (i > 0)
            {
                Vector3 prevPos = childObjectList[i - 1].position;
                Gizmos.DrawLine(prevPos,currentPos);
            }
        }
    }
}
