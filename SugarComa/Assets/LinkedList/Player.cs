using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{

    [SerializeField] public Node node;
    [SerializeField] pathFinder pathfinder;
    public int steps;
    public int speed;
    public bool isMoving;
    public bool isSelecting, isSelected;
    [SerializeField] public Material greenMaterial, redMaterial;
    void Update()
    {
        if (!isSelecting)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
            {
                steps = 3;
                StartCoroutine(Move("normal"));
            }
            else if (Input.GetKeyDown(KeyCode.X) && !isMoving)
            {
                pathfinder.FindBestPath(node, "goal");
                StartCoroutine(Move("goal"));               
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectNode("left");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                selectNode("right");
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter) && isSelected)
            {
                selectNode("clear");
                StartCoroutine(Move("normal"));
            }
        }

    }
    public bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }
    public IEnumerator Move(string type)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        if (type.Equals("goal"))
        {
            int i = 0;
            while (node != pathfinder.foundnode)
            {
                Vector3 nextPos = pathfinder.paths[i].nodePos;
                i++;
                while (MoveToNextNode(nextPos)) { yield return null; }
                node = pathfinder.paths[i-1];
            }
        }//Kupa kazanacaðýmýz yer gibi yerler için test.
        else if (type.Equals("normal"))
        {
            while (steps>0 && !isSelecting)
            {
                Vector3 nextPos = node.selectionNext.nodePos;
                while (MoveToNextNode(nextPos)) { yield return null; }
                if (node.next.isSelector)
                {
                    isSelecting = true;
                    node.goToNextNode();
                    steps--;
                    yield break;
                }

                node.goToNextNode();
                steps--;
            }
        }
        isMoving = false;
        pathfinder.ResetPathFinder(isMoving);
    }

    void selectNode(string type)
    {
        switch (type)
        {
            case "left":
                node.left.material = greenMaterial;
                node.right.material = redMaterial;
                node.selectionNext = node.leftChoice;
                isSelected = true;
                break;
            case "right":
                node.right.material = greenMaterial;
                node.left.material = redMaterial;
                node.selectionNext = node.rightChoice;
                isSelected = true;
                break;
            default:
                node.right.material = redMaterial;
                node.left.material = redMaterial;
                isSelecting = false;
                isMoving = false;
                isSelected = false;
                break;
        }
    }
}
