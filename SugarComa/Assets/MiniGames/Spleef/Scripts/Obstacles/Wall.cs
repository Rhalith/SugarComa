using Assets.MiniGames.HoleInTheWall.Scripts.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private WallChoice _wallChoice;
    
    public void DisableWall()
    {
        gameObject.SetActive(false);
        _wallChoice.ChooseWall();
    }
}
