using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFinder : MonoBehaviour
{
    public List<Node> paths;
    public List<List<Node>> nodes = new() { new()};
    public Node next, alternate, foundnode, previous, current;
    [SerializeField] Player player;
    public bool isFound;

    public void Founded(Node node)
    {
        foundnode = node;
        isFound = true;
    }
    public void FindSpec(Node node, string target)
    {
        if(node.spec != null && node.spec.Equals(target))
        {
            print(node);
            return;
        }
        var path = nodes[^1];
        if (path.Contains(node.next)) return;
        var length = nodes.Count - 1;
        for (int i = 0; i < length; i++)
        {
            if (nodes[i].Contains(node.next)) return;
        }
        if(node.next != null)
        {
            path.Add(node.next);
            FindSpec(node.next, target);
        }
        if(node.alternate != null)
        {
            List<Node> alternatePath = new();
            for (int i = 0; i < path.Count; i++)
            {
                alternatePath.Add(path[i]);
                if (path[i].isSelector)
                {
                    path[i].isSelector = false; 
                    break;
                }
            }
            nodes.Add(alternatePath);
            alternatePath.Add(node.alternate);
            FindSpec(node.alternate, target);
        }
    }
    public void FindBestPath(Node node, string target)
    {
        FindSpec(node, target);
        int bestCount = int.MaxValue;
        int index = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (bestCount > nodes[i].Count)
            {
                index = i;
                bestCount = nodes[i].Count;
            }
        }
        paths = nodes[index];
        Founded(paths[^1]);
    }
    public void ResetSelector(List<List<Node>> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Count; j++)
            {
                if (list[i][j].spec.Equals("selection"))
                {
                    list[i][j].isSelector = true;
                }
            }
        }
    }
    public void ResetPathFinder(bool isPlayerMoving)
    {
        if (!isPlayerMoving)
        {
            paths.Clear();
            foundnode = null;
            isFound = false;
            ResetSelector(nodes);
            nodes.Clear();
            nodes.Add(new());
        }

    }
}
//void nodeChanger(string type)
//{
//    if (type.Equals("alternate"))
//    {
//        alternate = alternate.alternate;
//        next = next.next;
//    }
//    else
//    {
//        alternate = next.alternate;
//        next = next.next;
//    }
//}
//void goBackwards(Node current)
//{
//    if(current != player.node)
//    {
//        paths.Add(current);
//        if (current.alternateprevious != null && !isSearched)
//        {
//            Node mynode;
//            mynode = current;
//            if (current == mynode)
//            {
//                isSearched = true;
//                mynode = null;
//            }
//            current = current.alternateprevious;
//        }
//        current = current.previous;
//        goBackwards(current);
//    }
//    else
//    {
//        paths.Reverse();
//    }
//}
//private void Update()
//{
//    if (Input.GetKeyDown(KeyCode.K))
//    {
//        print(next);
//        print(next.next);
//        print(alternate);
//        print(next.alternate);
//    }
//}
//public void findSpec(string target)
//{
//    if(next != player.node && !isNextPlayer)
//    {
//        next = player.node.next;
//        alternate = next.alternate;
//        if (!isFound)
//        {
//            isNextPlayer = true;
//        }
//    }
//    if (!isFound)
//    {
//        if (next.isSelector)
//        {
//            print("selection");
//            if (next.alternate.spec.Equals("goal"))
//            {
//                print("goal");
//                leftDepth++;
//                isFound = true;
//            }
//            else
//            {
//                print("notgoal");
//                nodeChanger("next");
//                rightDepth++;
//                isRoadSplitted = true;
//            }
//        }
//        else if (isRoadSplitted)
//        {   
//            nodeChanger("alternate");
//            if (next.spec.Equals("goal"))
//            {
//                print("goal");
//                leftDepth++;
//                Founded(next);
//            }
//            else
//            {
//                leftDepth++;
//            }
//            if (alternate.spec.Equals("goal"))
//            {
//                print("goal");
//                rightDepth++;
//                Founded(alternate);
//            }
//            else
//            {
//                rightDepth++;
//            }
//        }
//        else
//        {
//            nodeChanger("next");
//        }
//    }
//    else
//    {
//        goBackwards(foundnode);
//        return;
//    }
//    findSpec(target);
//}