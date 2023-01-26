using Assets.MiniGames.Basketball.Scripts.Ball;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDetector : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro m_TextMeshPro;
    [SerializeField] private Animator _netAnimator;
    public float Score;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Score++;
            m_TextMeshPro.text = "00"+Score.ToString();
            _netAnimator.SetBool("isShot",true);
        }
    }
    private void ResetAnimation()
    {
        _netAnimator.SetBool("isShot", false);
    }
}
