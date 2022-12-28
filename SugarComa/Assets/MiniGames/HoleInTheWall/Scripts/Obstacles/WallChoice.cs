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
            switch(i)
            {
                case 0:
                    _walls[0].gameObject.SetActive(true);
                    break;
                case 1:
                    _walls[1].gameObject.SetActive(true);
                    break;
                case 2:
                    _walls[2].gameObject.SetActive(true);
                    break;
                default:
                    _walls[3].gameObject.SetActive(true);
                    break;
            }
        }
    }
}