using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinkedList
{
public class Player : MonoBehaviour
{
    [Serializable]
    public class Selection
    {
        public bool isSelecting, isSelected;
        public Material greenMaterial, redMaterial;
    }
    public Node node;
    
    public int health;
    public int gold;
    public int steps;
    public int speed;
    public bool isMoving;
    [Header("Di�er Scriptler")]
    [SerializeField] pathFinder pathfinder;
    [SerializeField] GameController gameController;
    [Header("Se�im Ayarlar�")]
    public Selection selection;

    void Update()
    {
        if (!selection.isSelecting)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
            {
                steps = 3;
                StartCoroutine(Move("normal"));
            }
            else if (Input.GetKeyDown(KeyCode.X) && !isMoving)
            {
                pathfinder.FindBestPath(node, Node.Specification.goal);
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
            if (Input.GetKeyDown(KeyCode.KeypadEnter) && selection.isSelected)
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
        }//Kupa kazanaca��m�z yer gibi yerler i�in test.
        else if (type.Equals("normal"))
        {
            while (steps>0 && !selection.isSelecting)
            {
                Vector3 nextPos = node.selectionNext.nodePos;
                while (MoveToNextNode(nextPos)) { yield return null; }
                if (node.next.selection.isSelector)
                {
                    selection.isSelecting = true;
                    node.goToNextNode();
                    steps--;
                    yield break;
                }

                node.goToNextNode();
                steps--;
            }
        }
        isMoving = false;
        CheckCurrentNode(node);
        pathfinder.ResetPathFinder(isMoving);
    }

    void selectNode(string type)
    {
        switch (type)
        {
            case "left":
                node.selection.left.material = selection.greenMaterial;
                node.selection.right.material = selection.redMaterial;
                node.selectionNext = node.selection.leftChoice;
                selection.isSelected = true;
                break;
            case "right":
                node.selection.right.material = selection.greenMaterial;
                node.selection.left.material = selection.redMaterial;
                node.selectionNext = node.selection.rightChoice;
                selection.isSelected = true;
                break;
            default:
                node.selection.right.material = selection.redMaterial;
                node.selection.left.material = selection.redMaterial;
                selection.isSelecting = false;
                isMoving = false;
                selection.isSelected = false;
                break;
        }
    }
    void CheckCurrentNode(Node node)
    {
        switch (node.specification)
        {
            case Node.Specification.gold:
                addGold(5);
                break;
            case Node.Specification.heal:
                addHealth(5);
                break;
        }
    }
    void addGold(int value)
    {
        gold += value;
        gameController.ChangeText();
    }
    void addHealth(int value)
    {
        health += value;
        if (health > 30)
        {
            health = 30;
        }
        gameController.ChangeText();
    }

        //void CheckNode(Node node)
        //{
        //    if(node.specification.Equals(Node.Specification.))
        //}

    }
}