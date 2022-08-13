using Assets.MiniGames.FallingStars.Scripts.Meteors;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.GameManaging
{
    public class MiniGameController : MonoBehaviour
    {
        #region Components

        #region Serialized Fields
        [SerializeField] private Meteor _meteorPrefab;
        [SerializeField] private MiniGameManager _miniGameManager;
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] Borders _borders;
        #endregion

        private Queue<Meteor> AvaliableMeteors = new Queue<Meteor>(11);

        public static MiniGameController Instance { get; private set; }
        
        #endregion



        private void Awake()
        {
            Instance = this;
            //GrowPool();
            _miniGameManager.SpawnNewWave += SpawnWave;
        }
        private void OnDisable()
        {
            _miniGameManager.SpawnNewWave -= SpawnWave;
        }
/*
        private void GrowPool()
        {
            for (int i = 0; i < 20; i++)
            {
                Meteor instanceToAdd = Instantiate(_meteorPrefab);
                instanceToAdd.transform.SetParent(transform);
                AddToPool(instanceToAdd);
            }
        }
        */
        public void AddToPool(Meteor instance)
        {/*
            instance.gameObject.SetActive(false);
            AvaliableMeteors.Enqueue(instance);*/
            _poolManager.AddToPool(instance.gameObject);
        }

        public Meteor GetFromPool()
        {/*
            if (AvaliableMeteors.Count == 0)
            {
                GrowPool();
            }

            Meteor instance = AvaliableMeteors.Dequeue();
            instance.GetComponent<Meteor>().SetType();
            instance.gameObject.SetActive(true);
            return instance;*/
            Meteor instance = _poolManager.GetFromPool(0).GetComponent<Meteor>();
            instance.SetType();
            return instance;
        }

        public void SpawnWave()
        {
            int meteorCount = _miniGameManager.MeteorCount;
            print(meteorCount);
            for (int i = 0; i < meteorCount; i++)
            {
                float x = Random.Range(_borders._leftBorder.position.x, _borders._rightBorder.position.x);
                float z = Random.Range(_borders._upBorder.position.z, _borders._bottomBorder.position.z);
                //float z = GetZValue(x,_borders._leftBorder.position.x);
                print("x -> " + x + " z -> " + z);
                Meteor instance = GetFromPool();
                instance.transform.position = new Vector3(x, 0, z);
                instance.MeteorShadow.SetActive(true);

                CheckMeteorPosition(instance, x, z);
               StartCoroutine(ActivateObject(instance));
            }
        }

        private float GetZValue(float x, float radius)
        {
            float zValue = Mathf.Pow((Mathf.Pow(radius, 2) - Mathf.Pow(x, 2)), 0.5f);
            return Random.Range(-zValue,zValue);
        }

        private void CheckMeteorPosition(Meteor meteor, float x, float z)
        {
            if(meteor.gameObject.GetComponentInChildren<MeteorShadow>().IsIn)
            {
                meteor.gameObject.GetComponentInChildren<MeteorShadow>().IsIn = false;
                meteor.transform.position = new Vector3(x, 0, z);
            }
        }
        private IEnumerator ActivateObject(Meteor gameObject)
        {
            yield return new WaitForSeconds(1f);
            gameObject.MeteorObject.SetActive(true);
        }
    }
}

//[SerializeField] float _gameTime;
//[SerializeField] TMP_Text timeText;
//private float lastUpdateMeteorCount;
//[SerializeField] private GameObject[] meteorGameObject;
//[SerializeField] private GameObject[] meteorShadows;
//public int meteorCount = 3;
//public float updateMeteorCountTime = 15f;
//private float lastMeteorWave;
//private bool meteorWaveStarted = false;
//private bool meteorWaveCalculateStarted = false;
//[SerializeField] private float meteorWaveSpawnTime;
//[SerializeField] private float meteorWaveCalculateTime;
//public float meteorVelocity = 5f;
//public float planeWidth = 20f;
//private void Start()
//{
//    lastUpdateMeteorCount = Time.time;
//    lastMeteorWave = Time.time;
//    StartCoroutine(StartCountdown());
//}
//private void Update()
//{
//    if (_gameTime - lastUpdateMeteorCount > updateMeteorCountTime)
//    {
//        lastUpdateMeteorCount = _gameTime;
//        meteorCount++;
//    }

//    if (!meteorWaveCalculateStarted && _gameTime - lastMeteorWave > meteorWaveCalculateTime)
//    {
//        print("meteorWaveCalculateStarted");
//        meteorWaveCalculateStarted = true;
//        calculateNewWave();
//        StartCoroutine(WaveCalculate());
//    }

//    if (!meteorWaveStarted && _gameTime - lastMeteorWave > meteorWaveSpawnTime)
//    {
//        print("meteorWaveStarted");
//        meteorWaveStarted = true;
//        startNewWave();
//        StartCoroutine(WaveControl());
//    }
//}

//IEnumerator WaveControl()
//{
//    yield return new WaitForSeconds(meteorWaveSpawnTime);
//    startNewWave();
//    StartCoroutine(WaveControl());
//}

//IEnumerator WaveCalculate()
//{
//    yield return new WaitForSeconds(meteorWaveSpawnTime);
//    calculateNewWave();
//    StartCoroutine(WaveCalculate());
//}

//private void calculateNewWave()
//{
//    for (int i = 0; i < meteorCount; i++)
//    {
//        float x = Random.Range(-planeWidth + meteorVelocity * 2, planeWidth + meteorVelocity * 2);
//        float z = Random.Range(-planeWidth + meteorVelocity * 2, planeWidth + meteorVelocity * 2);
//        meteorGameObject[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
//        meteorGameObject[i].transform.position = new Vector3(x, 20f, z);
//        meteorGameObject[i].SetActive(true);
//        meteorGameObject[i].GetComponent<Rigidbody>().useGravity = false;
//        meteorShadows[i].SetActive(true);
//        meteorShadows[i].transform.position = new Vector3(x - meteorVelocity * 2, 0f, z - meteorVelocity * 2);
//    }
//}

//private void startNewWave()
//{
//    for (int i = 0; i < meteorCount; i++)
//    {
//        meteorGameObject[i].GetComponent<Rigidbody>().velocity = new Vector3(-meteorVelocity, 0, -meteorVelocity);
//        meteorGameObject[i].GetComponent<Rigidbody>().useGravity = true;
//    }
//}

//public IEnumerator StartCountdown()
//{
//    string text = timeText.text.ToString();
//    while (_gameTime >= 0)
//    {
//        timeText.SetText(text + _gameTime.ToString());
//        if (_gameTime == 0)
//        {
//            timeText.SetText(text + _gameTime.ToString().AddColor(Color.red));
//        }
//        yield return new WaitForSeconds(1.0f);
//        _gameTime--;
//    }
//}