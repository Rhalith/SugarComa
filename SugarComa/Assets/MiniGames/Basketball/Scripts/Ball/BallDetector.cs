using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.Basketball.Scripts.Ball
{
    public class BallDetector : MonoBehaviour
    {
        [SerializeField] private DetectType detectType;
        #region NetAttributes
        [Header("Net Attributes")]
        [SerializeField] TMPro.TextMeshPro m_TextMeshPro;
        [SerializeField] private Animator _netAnimator;
        [SerializeField] private GameObject _goalEffect;
        public float Score;
        #endregion
        #region ReadyAttributes
        [Header("Ready Attributes")]
        [SerializeField] private BallManager _ballManager;
        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                if (detectType.Equals(DetectType.Net))
                {
                    ScoreBall();
                }
                else
                {
                    ReadyBall(other);
                }
            }
        }

        private void ResetAnimation()
        {
            _netAnimator.SetBool("isShot", false);
            _goalEffect.SetActive(false);
        }

        private void ScoreBall()
        {
            _netAnimator.SetBool("isShot", true);
            _goalEffect.SetActive(true);
            Score++;
            m_TextMeshPro.text = Score.ToString().PadLeft(3,'0');
            Invoke(nameof(ResetAnimation), 0.2f);
        }
        private void ReadyBall(Collider other)
        {
            other.GetComponent<BallMovement>().IsReady = true;
        }
    }

    enum DetectType
    {
        Net,
        Ready
    }
}