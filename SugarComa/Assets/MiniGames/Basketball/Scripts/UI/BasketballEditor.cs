using Assets.MiniGames.Basketball.Scripts.Ball;
using UnityEditor;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.UI
{
    [CustomEditor(typeof(BallMovement))]

    public class BasketballEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BallMovement myScript = (BallMovement)target;
            if (GUILayout.Button("ResetBallPosition"))
            {
                myScript.ResetBall();
            }
            else if (GUILayout.Button("ThrowBall"))
            {
                myScript.ThrowBallVector();
            }
        }
    }
}