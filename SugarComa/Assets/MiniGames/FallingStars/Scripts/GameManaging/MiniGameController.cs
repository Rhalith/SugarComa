using Assets.MiniGames.FallingStars.Scripts.Meteors;
using Assets.MiniGames.FallingStars.Scripts.Meteors.MeteorObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.GameManaging
{
    public class MiniGameController : MonoBehaviour
    {
        #region Components

        [SerializeField] private Meteor meteorPrefab;
        [SerializeField] private MiniGameManager miniGameManager;

        private Queue<Meteor> AvaliableMeteors = new Queue<Meteor>(11);

        public static MiniGameController Instance { get; private set; }
        [SerializeField] Borders _borders;
        #endregion



        private void Awake()
        {
            Instance = this;
            GrowPool();
            miniGameManager._SpawnNewWave += SpawnWave;
        }
        private void OnDisable()
        {
            miniGameManager._SpawnNewWave -= SpawnWave;
        }

        private void GrowPool()
        {
            for (int i = 0; i < 20; i++)
            {
                Meteor instanceToAdd = Instantiate(meteorPrefab);
                instanceToAdd.transform.SetParent(transform);
                AddToPool(instanceToAdd);
            }
        }

        public void AddToPool(Meteor instance)
        {
            instance.gameObject.SetActive(false);
            AvaliableMeteors.Enqueue(instance);
        }

        public Meteor GetFromPool()
        {
            if (AvaliableMeteors.Count == 0)
            {
                GrowPool();
            }

            Meteor instance = AvaliableMeteors.Dequeue();
            instance.GetComponent<Meteor>().SetType();
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void SpawnWave()
        {
            print(miniGameManager.MeteorCount);
            for (int i = 0; i < miniGameManager.MeteorCount; i++)
            {
                float x = Random.Range(_borders._leftBorder.position.x, _borders._rightBorder.position.x);
                float z = Random.Range(_borders._upBorder.position.z, _borders._bottomBorder.position.z);
                Meteor instance = GetFromPool();
                instance.transform.position = new Vector3(x, 0, z);
                instance._meteorShadow.SetActive(true);
                CheckMeteorPosition(instance, x, z);
                StartCoroutine(ActivateObject(instance));
            }
        }

        private void CheckMeteorPosition(Meteor meteor, float x, float z)
        {
            if(meteor.gameObject.GetComponentInChildren<MeteorShadow>().isIn)
            {
                meteor.gameObject.GetComponentInChildren<MeteorShadow>().isIn = false;
                meteor.transform.position = new Vector3(x, 0, z);
            }
        }
        private IEnumerator ActivateObject(Meteor gameObject)
        {
            yield return new WaitForSeconds(1f);
            gameObject._meteorObject.SetActive(true);
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