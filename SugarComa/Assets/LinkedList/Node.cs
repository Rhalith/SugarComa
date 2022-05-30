using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour
{
    [System.Serializable]
    public class Selection
    {
        public Node leftChoice, rightChoice;
        public MeshRenderer left, right;
        public bool isSelector;
    }
    public enum Specification
    {
        empty,
        gold,
        goal,
        selection,
        heal,
        gift,
        jackpot,
        trap_1,
        trap_2,
        trap_3
    }
    [Tooltip("Özelliði yoksa boþ býrak!")]
    public Specification specification;
    [HideInInspector]
    public Vector3 nodePos;
    public Node current, previous, next, alternate, alternateprevious, selectionNext;

    [Header("Diðer Scriptler")]
    [SerializeField] pathFinder pathfinder;
    [SerializeField] Player player;


    [Header("Seçim Ayarlarý")]
    public Selection selection;



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
        switch (specification)
        {
            case Specification.selection:
                //blabla
                break;
            case Specification.gold:
                //blabla
                break;
            default:
                break;
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

