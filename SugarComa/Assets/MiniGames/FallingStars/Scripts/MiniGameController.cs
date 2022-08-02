using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts
{
    public class MiniGameController : MonoBehaviour
    {
        [SerializeField] float _gameTime;
        [SerializeField] TMP_Text timeText;
        private float lastUpdateMeteorCount;
        [SerializeField] private GameObject[] meteorGameObject;
        [SerializeField] private GameObject[] meteorShadows;
        public int meteorCount = 3;
        public float updateMeteorCountTime = 15f;
        private float lastMeteorWave;
        private bool meteorWaveStarted = false;
        private bool meteorWaveCalculateStarted = false;
        [SerializeField] private float meteorWaveSpawnTime;
        [SerializeField] private float meteorWaveCalculateTime;
        public float meteorVelocity = 5f;
        public float planeWidth = 20f;
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

            if (!meteorWaveCalculateStarted && _gameTime - lastMeteorWave > meteorWaveCalculateTime)
            {
                print("meteorWaveCalculateStarted");
                meteorWaveCalculateStarted = true;
                calculateNewWave();
                StartCoroutine(WaveCalculate());
            }

            if (!meteorWaveStarted && _gameTime - lastMeteorWave > meteorWaveSpawnTime)
            {
                print("meteorWaveStarted");
                meteorWaveStarted = true;
                startNewWave();
                StartCoroutine(WaveControl());
            }
        }

        IEnumerator WaveControl()
        {
            yield return new WaitForSeconds(meteorWaveSpawnTime);
            startNewWave();
            StartCoroutine(WaveControl());
        }

        IEnumerator WaveCalculate()
        {
            yield return new WaitForSeconds(meteorWaveSpawnTime);
            calculateNewWave();
            StartCoroutine(WaveCalculate());
        }

        private void calculateNewWave()
        {
            for (int i = 0; i < meteorCount; i++)
            {
                float x = Random.Range(-planeWidth + meteorVelocity, planeWidth + meteorVelocity);
                float z = Random.Range(-planeWidth + meteorVelocity, planeWidth + meteorVelocity);
                meteorGameObject[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                meteorGameObject[i].transform.position = new Vector3(x, 20f, z);
                meteorGameObject[i].SetActive(true);
                meteorGameObject[i].GetComponent<Rigidbody>().useGravity = false;
                meteorShadows[i].SetActive(true);
                meteorShadows[i].transform.position = new Vector3(x - 10f, 0f, z - 10f);
            }
        }

        private void startNewWave()
        {
            for (int i = 0; i < meteorCount; i++)
            {
                meteorGameObject[i].GetComponent<Rigidbody>().velocity = new Vector3(-meteorVelocity, 0, -meteorVelocity);
                meteorGameObject[i].GetComponent<Rigidbody>().useGravity = true;
            }
        }

        public IEnumerator StartCountdown()
        {
            string text = timeText.text.ToString();
            while (_gameTime >= 0)
            {
                timeText.SetText(text + _gameTime.ToString());
                if (_gameTime == 0)
                {
                    timeText.SetText(text + _gameTime.ToString().AddColor(Color.red));
                }
                yield return new WaitForSeconds(1.0f);
                _gameTime--;
            }
        }
    }
}