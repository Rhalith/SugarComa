using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Router currentRouter;

    public int routePosition;

    public int steps;

    public float speed;

    public bool isMoving;

    public bool isSelecting;

    public bool overrideSelection;


    public void startMove()
    {
        if (routePosition + steps < currentRouter.childObjectList.Count || overrideSelection)
        {
            StartCoroutine(Move());
            Debug.Log("Normal");
        }
        else
        {
            StartCoroutine(MoveToLast());
            Debug.Log("toLast");
        }
    }

    IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        while(steps > 0 && !isSelecting)
        {
            routePosition++;
            routePosition %= currentRouter.childObjectList.Count;

            Vector3 nextPos = currentRouter.childObjectList[routePosition].position;
            while (MoveToNextNode(nextPos)) 
            {
                print("test");
                yield return null; 
            }
        }

        isMoving = false;
    }
    IEnumerator MoveToLast()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        while (steps > 0 && !isSelecting)
        {
            Vector3 nextPos = currentRouter.lastObject.position;
            while (MoveToNextNode(nextPos)) { print("test"); yield return null; }

            routePosition++;
        }
        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }
}
