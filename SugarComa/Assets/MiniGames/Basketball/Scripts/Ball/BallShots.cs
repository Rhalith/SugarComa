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
                new Vector3(0, 2f, 1.3f),
                new Vector3(0, 2f, 1.5f),
                new Vector3(0, 2.1f, 1.1f),
                new Vector3(0, 2.1f, 1.3f),
                new Vector3(0, 2.1f, 1.6f),
                new Vector3(0, 2.2f, 1f),
                new Vector3(0, 2.2f, 1.2f),
                new Vector3(0, 2.3f, 1f),
                new Vector3(0, 2.4f, 1f),
                new Vector3(0, 2.5f, 0.9f),
            };
        }
        private void PrepareAirBalls()
        {
            _airBalls = new List<Vector3>
            {
                new Vector3(0, 2f, 1.4f),
                new Vector3(0, 2f, 1.8f),
                new Vector3(0, 2.1f, 1.2f),
                new Vector3(0, 2.1f, 1.7f),
                new Vector3(0, 2.2f, 1.1f),
                new Vector3(0, 2.2f, 1.3f),
                new Vector3(0, 2.3f, 0.9f),
                new Vector3(0, 2.3f, 1.2f),
                new Vector3(0, 2.4f, 1f),
                new Vector3(0, 2.5f, 0.8f),
            };
        }
        private void PrepareBricks()
        {
            _bricks = new List<Vector3>
            {
                new Vector3(0, 2f, 1f),
                new Vector3(0, 2f, 1.2f),
                new Vector3(0, 2.1f, 0.7f),
                new Vector3(0, 2.1f, 2.2f),
                new Vector3(0, 2.2f, 0.6f),
                new Vector3(0, 2.3f, 0.7f),
                new Vector3(0, 2.3f, 0.8f),
                new Vector3(0, 2.4f, 0.7f),
                new Vector3(0, 2.4f, 1.2f),
                new Vector3(0, 2.5f, 0.6f),
            };
        }
    }
}