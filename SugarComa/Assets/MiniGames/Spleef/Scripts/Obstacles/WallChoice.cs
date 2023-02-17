using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.HoleInTheWall.Scripts.Obstacles
{
    public class WallChoice : MonoBehaviour
    {
        [SerializeField] private List<Wall> _walls;
        private void Start()
        {
            ChooseWall();
        }
        public void ChooseWall()
        {
            int i = Random.Range(0, _walls.Count);
            _walls[i].gameObject.SetActive(true);
        }
    }
}