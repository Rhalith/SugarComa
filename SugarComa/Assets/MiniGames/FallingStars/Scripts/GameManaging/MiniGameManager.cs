using Assets.MiniGames.FallingStars.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.MiniGames.FallingStars.Scripts.GameManaging
{
    public static class StringExtensions
    {
        public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
        public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
    }
    public class MiniGameManager : MonoBehaviour
    {

        #region Components
        private static MiniGameManager _instance;

        #region SerializeFields
        [SerializeField] private MeteorMeshes _meteorMeshes;
        [SerializeField] private MeteorEffectMeshes _meteorEffectMeshes;
        [SerializeField] private MeteorMaterials _meteorMaterials;
        [SerializeField] private TMP_Text _timeText;
        #endregion
        #endregion

        #region Properties
        public Action SpawnNewWave;
        public int MeteorCount => _meteorCount;
        private float _gameTime = 120f;
        private int _meteorCount = 3;

        #region SerializeFields
        [SerializeField] private int _meteorCountUpdateTime = 15;
        [SerializeField] private int _meteorWaveSpawnTime = 4;
        #endregion
        #endregion

        public static MeteorMeshes MeteorMeshes
        {
            get
            {
                if (_instance == null) return null;
                return _instance._meteorMeshes;
            }
        }
        public static MeteorEffectMeshes MeteorEffectMeshes
        {
            get
            {
                if (_instance == null) return null;
                return _instance._meteorEffectMeshes;
            }
        }

        public static MeteorMaterials MeteorMaterials
        {
            get
            {
                if (_instance == null) return null;
                return _instance._meteorMaterials;
            }
        }
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Invoke(nameof(StartFirstWave), 2);
            Invoke(nameof(FirstUpdateMeteorCount), _meteorCountUpdateTime);
            StartCoroutine(StartCountdown());
        }

        IEnumerator SpawnWave()
        {
            while(true)
            {
                yield return new WaitForSeconds(_meteorWaveSpawnTime);
                SpawnNewWave?.Invoke();
            }
        }
        //TODO add if gametime >= 0 to check.
        IEnumerator UpdateMeteorCount()
        {
            _meteorCount++;
            yield return new WaitForSeconds(_meteorCountUpdateTime);
            StartCoroutine(UpdateMeteorCount());
        }
        IEnumerator StartCountdown()
        {
            string text = _timeText.text.ToString();
            while (_gameTime >= 0)
            {
                _timeText.SetText(text + _gameTime.ToString());
                if (_gameTime == 0)
                {
                    _timeText.SetText(text + _gameTime.ToString().AddColor(Color.red));
                }
                yield return new WaitForSeconds(1.0f);
                _gameTime--;
            }
        }
        private void StartFirstWave()
        {
            StartCoroutine(SpawnWave());
            SpawnNewWave?.Invoke();

        }
        private void FirstUpdateMeteorCount()
        {
            StartCoroutine(UpdateMeteorCount());
        }
    }
}

//public class FallingStarsManager : MonoBehaviour
//{
//    private float _gameTime;
//    [SerializeField] TMP_Text timeText;
//    private float lastUpdateMeteorCount;
//    public int meteorCount = 3;
//    public float updateMeteorCountTime = 15f;
//    private float lastMeteorWave;
//    [SerializeField] private float meteorWaveSpawnTime;
//    [SerializeField] private float meteorWaveCalculateTime;
//    [SerializeField] private GameObject[] meteorGameObject;
//    [SerializeField] private GameObject[] meteorShadows;
//    public float meteorVelocity = 5f;
//    public float planeWidth = 20f;
//    private bool meteorWaveStarted = false;
//    private bool meteorWaveCalculateStarted = false;


//    private void Start()
//    {
//        lastUpdateMeteorCount = Time.time;
//        lastMeteorWave = Time.time;

//    }
//    private void Update()
//    {
//        _gameTime = Time.time;
//        timeText.text = (_gameTime).ToString();
//        if (_gameTime - lastUpdateMeteorCount > updateMeteorCountTime)
//        {
//            lastUpdateMeteorCount = _gameTime;
//            meteorCount++;
//        }

//        if (!meteorWaveCalculateStarted && (_gameTime - lastMeteorWave) > meteorWaveCalculateTime)
//        {
//            print("meteorWaveCalculateStarted");
//            meteorWaveCalculateStarted = true;
//            calculateNewWave();
//            StartCoroutine(WaveCalculate());
//        }

//        if (!meteorWaveStarted && (_gameTime - lastMeteorWave) > meteorWaveSpawnTime)
//        {
//            print("meteorWaveStarted");
//            meteorWaveStarted = true;
//            startNewWave();
//            StartCoroutine(WaveControl());
//        }
//    }

//    IEnumerator WaveControl()
//    {
//        yield return new WaitForSeconds(meteorWaveSpawnTime);
//        startNewWave();
//        StartCoroutine(WaveControl());
//    }

//    IEnumerator WaveCalculate()
//    {
//        yield return new WaitForSeconds(meteorWaveSpawnTime);
//        calculateNewWave();
//        StartCoroutine(WaveCalculate());
//    }

//    private void calculateNewWave()
//    {
//        for (int i = 0; i < meteorCount; i++)
//        {
//            float x = Random.Range(-planeWidth + meteorVelocity, planeWidth + meteorVelocity);
//            float z = Random.Range(-planeWidth + meteorVelocity, planeWidth + meteorVelocity);
//            meteorGameObject[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
//            meteorGameObject[i].transform.position = new Vector3(x, 20f, z);
//            meteorGameObject[i].SetActive(true);
//            meteorGameObject[i].GetComponent<Rigidbody>().useGravity = false;
//            meteorShadows[i].SetActive(true);
//            meteorShadows[i].transform.position = new Vector3(x - 10f, 0f, z - 10f);

//        }
//    }

//    private void startNewWave()
//    {
//        for (int i = 0; i < meteorCount; i++)
//        {
//            meteorGameObject[i].GetComponent<Rigidbody>().velocity = new Vector3(-meteorVelocity, 0, -meteorVelocity);
//            meteorGameObject[i].GetComponent<Rigidbody>().useGravity = true;
//        }
//    }
//}