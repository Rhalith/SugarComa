using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.Ball
{
    public class BallShots
    {
        private List<Vector3> _goals, _airBalls, _bricks;

        public BallShots()
        {
            PrepareShots();
        }

        private void PrepareShots()
        {
            PrepareGoals();
            PrepareAirBalls();
            PrepareBricks();
        }
        public Vector3 GetGoal()
        {
            int i = Random.Range(0, _goals.Count);
            return _goals[i];
        }
        public Vector3 GetAirBall()
        {
            int i = Random.Range(0, 2);
            if(i == 0)
            {
                int j = Random.Range(0, _goals.Count);
                return _goals[j];
            }
            else
            {
                int j = Random.Range(0, _airBalls.Count);
                return _airBalls[j];
            }

        }
        public Vector3 GetBrick()
        {
            int i = Random.Range(0, _bricks.Count);
            return _bricks[i];
        }

        private void PrepareGoals()
        {
            _goals = new List<Vector3>
            {
                new Vector3(0, 9.3f, 4.5f),
                new Vector3(0, 9.4f, 4.5f),
                new Vector3(0, 9.5f, 4.5f),
                new Vector3(0, 9.8f, 4.5f),
                new Vector3(0, 10f, 4.5f),
                new Vector3(-0.1f, 9.5f, 5f),
                new Vector3(-0.2f, 9.5f, 5f),
                new Vector3(-0.3f, 9.5f, 5f),
                new Vector3(0, 9.5f, 5f),
                new Vector3(0.3f, 9.5f, 5f),
                new Vector3(0.3f, 9.4f, 4.5f),
                new Vector3(0.4f, 9.5f, 5f),
                new Vector3(0, 9.5f, 5.2f),
                new Vector3(0, 9.5f, 5.4f),
                new Vector3(0, 9.5f, 5.6f),                
                new Vector3(-0.8f, 9f, 5f),
                new Vector3(-0.9f, 9f, 5f),
                new Vector3(0,8f,25f),
            };
        }
        private void PrepareAirBalls()
        {
            _airBalls = new List<Vector3>
            {
                new Vector3(0.1f, 9.5f, 5f),
                new Vector3(0.2f, 9.5f, 5f),
                new Vector3(0.7f, 9.5f, 5f),
                new Vector3(-0.7f, 9.5f, 5f),
                new Vector3(-0.3f, 9.3f, 4.5f),
                new Vector3(0, 9f, 4.5f),
                new Vector3(0.2f, 9.2f, 4.5f),
                new Vector3(0f, 9.2f, 4.3f),
                new Vector3(0f, 9.2f, 4.4f),
                new Vector3(0.2f, 9.4f, 4.5f),
                new Vector3(0.3f, 9.2f, 4.5f),
                new Vector3(0.3f, 9.2f, 5f),
                new Vector3(-0.2f, 9.2f, 5f),
                new Vector3(0, 9f, 4.6f),
            };
        }
        private void PrepareBricks()
        {
            _bricks = new List<Vector3>
            {
                new Vector3(0.8f, 9f, 5f),
                new Vector3(0.9f, 9f, 5f),
                new Vector3(0f, 8f, 5f),
                new Vector3(0f, 8f, 4f),
                new Vector3(0f, 8f, 3f),
                new Vector3(0f, 9f, 4f),
                new Vector3(0.4f, 10f, 4f),
                new Vector3(-0.4f, 10f, 4f),
                new Vector3(0f, 6f, 8f),
                new Vector3(0.2f, 6f, 8f),
                new Vector3(-0.5f, 6f, 8f),
                new Vector3(0.8f, 8f, 12f),
                new Vector3(-0.8f, 8f, 12f),
            };
        }
    }
}