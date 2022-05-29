using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] pathFinder pathfinder;
    [SerializeField] public Node current, previous, next, alternate, alternateprevious, selectionNext;
    [SerializeField] Player player;
    [SerializeField] Vector3 playerpos;
    [Tooltip("Özelliði yoksa boþ býrak!")]
    public string spec; // Inspector'dan kendin doldur özelliðine göre.
    public Vector3 nodePos;
    [Tooltip("SEÇÝM YOKSA BOÞ BIRAK!")]
    [SerializeField] public Node leftChoice, rightChoice;
    [SerializeField] public MeshRenderer left, right;
    [SerializeField] public bool isSelector;
    public bool forStop;


    private void Awake()
    {
        current = this;
        selectionNext = next;
        nodePos = transform.position;
        setSpec();
    }
    //public void Print()
    //{
    //    Debug.Log("My pos: " + nodePos);
    //    if (next != null)
    //    {
    //        while (!next.spec.Equals("selection"))
    //        {
    //            Debug.Log(next.nodePos);
    //        }     
    //    }
    //}
    public void goToNextNode()
    {
        player.node = selectionNext;
    }
    public void checkNextNode()
    {
        pathfinder.next = next;
        pathfinder.alternate = alternate;
    }
    private void setSpec()
    {
        switch (spec)
        {
            case "selection":
                //blabla
                break;
            case "gold":
                //blabla
                break;
            default:
                break;
        }
           
    }
    public void MainRootFinder(Node node)
    {
        if (!node.forStop)
        {
            return;
        }
        node.forStop = false;
        current.GetComponent<MeshRenderer>().material = player.greenMaterial;
        if (node.next != null)
        {
            MainRootFinder(node.next);
        }
        if(node.alternate != null)
        {
            MainRootFinder(node.alternate);
        }
    }
    //public void FindSpec(string target, ref int i)
    //{
    //    i++;
    //    if (!pathfinder.firstSetted)
    //    {
    //        pathfinder.current = current;
    //        pathfinder.firstSetted = true;
    //    }
    //    if (!pathfinder.isFound)
    //    {
    //        if (!forStop)
    //        {
    //            forStop = true;
    //            current.GetComponent<MeshRenderer>().material = player.redMaterial;
    //            print(next + "and current: " + current);
    //            if (next.spec != null && next.spec.Equals(target))
    //            {
    //                pathfinder.Founded(next);
    //                GoBackWards(pathfinder.foundnode);
    //            }
    //            if (next != pathfinder.current)
    //            {
    //                if(alternate != null) alternate.FindSpec(target, ref i);
    //                next.FindSpec(target, ref i);
    //            }
    //        }   
    //    }
    //}
    //void GoBackWards(Node node)
    //{
    //    if (!pathfinder.isNextPlayer)
    //    {
    //        pathfinder.paths.Add(node);
    //        if(node.alternateprevious != null && node.previous != player.node)
    //        {
    //            node.GoBackWards(node.alternateprevious);
    //            node.previous.GoBackWards(node.previous);
    //        }
    //        else if (node.previous != player.node)
    //        {
    //            node.previous.GoBackWards(node.previous);
    //        }
    //        else
    //        {
    //            pathfinder.isNextPlayer = true;
    //            pathfinder.paths.Reverse();
    //            return;
                
    //        }
    //    }
    //    else
    //    {
            
            
    //    }
    //}
}
