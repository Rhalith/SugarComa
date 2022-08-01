using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MiniGames.FallingStars.Scripts
{
    public static class StringExtensions
    {
        public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
        public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
    }
    public class MiniGameManager : MonoBehaviour
    {
        [SerializeField] float _gameTime;
        [SerializeField] TMP_Text timeText;
        private float lastUpdateMeteorCount;
        public int meteorCount = 3;
        public float updateMeteorCountTime = 15f;
        private float lastMeteorWave;
        [SerializeField] private float meteorWaveTime;
        private void Start()
        {
            lastUpdateMeteorCount = Time.time;
            lastMeteorWave = Time.time;
            StartCoroutine(StartCountdown());
        }
        private void Update()
        {
            if (_gameTime - lastUpdateMeteorCount > updateMeteorCountTime)
            {
                lastUpdateMeteorCount = _gameTime;
                meteorCount++;
            }

            if (_gameTime - lastMeteorWave > 2f)
            {
                calculateNewWave();
            }

            if (_gameTime - lastMeteorWave > meteorWaveTime)
            {
                lastMeteorWave = Time.time;
                startNewWave();
            }
        }

        private void calculateNewWave()
        {

        }

        private void startNewWave()
        {

        }


        public IEnumerator StartCountdown()
        {
            string text = timeText.text.ToString();
            while (_gameTime >= 0)
            {
                timeText.SetText(text + _gameTime.ToString());
                if (_gameTime == 0)
                {
                    timeText.SetText(text + StringExtensions.AddColor(_gameTime.ToString(),Color.red));
                }
                yield return new WaitForSeconds(1.0f);
                _gameTime--;

            }

        }
    }

}