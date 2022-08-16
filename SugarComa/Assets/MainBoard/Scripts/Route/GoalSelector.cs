using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Utils.PlatformUtils;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.States;
using System.Linq;

namespace Assets.MainBoard.Scripts.Route
{
    public class GoalSelector : MonoBehaviour
    {
        [SerializeField] List<Platform> platforms;

        [SerializeField] CinemachineVirtualCamera _camera;

        [SerializeField] CinemachineBrain _cinemachineBrain;

        public GameObject _platformChangerObject;

        public GoalChestAnimation _chestAnimator;

        private int realPriority;

        private Platform _selectedPlatform;

        public bool isGoalActive;

        //TODO
        private GameObject _currentGoal;

        private Platform _currentPlatform;

        /// <summary>
        /// if there is a goal platform in map
        /// </summary>
        public static bool isAnyGoalPlatform;

        private void Start()
        {
            SteamServerManager.Instance.OnMessageReceived += OnMessageReceived;
        }

        private void OnDestroy()
        {
            SteamServerManager.Instance.OnMessageReceived -= OnMessageReceived;
        }

        private void OnMessageReceived(Steamworks.SteamId steamid, byte[] buffer)
        {
            if (NetworkHelper.TryGetChestData(buffer, out ChestNetworkData chestData))
            {
                CreateGoal(chestData.index);
            }
        }

        public void RandomGoalSelect()
        {
            int index = Random.Range(0, platforms.Count);

            // Kaldýr...
            // index = 1;

            // TODO: Check if player in that platform with currentplatform
            if (platforms[index].spec != PlatformSpec.Goal && !platforms[index].HasSelector)
            {
                if(_currentPlatform != null && platforms[index] == _currentPlatform)
                {
                    RandomGoalSelect();
                }

                bool result = SteamServerManager.Instance.SendingMessageToAll(NetworkHelper.Serialize(new ChestNetworkData((byte)index, MessageType.CreateChest)));

                if (result) CreateGoal(index);
            }
            else
            {
                RandomGoalSelect();
            }
        }

        private void CreateGoal(int index)
        {
            platforms[index].spec = PlatformSpec.Goal;
            CreateGoalObject(platforms[index]);
            VirtualCameraLookTo(_camera, platforms[index].transform);
            print(platforms[index]);
            isAnyGoalPlatform = true;
            PlayerStateContext.canPlayersAct = false;
            _currentPlatform = platforms[index];
        }

        private void VirtualCameraLookTo(CinemachineVirtualCamera camera, Transform target)
        {
            camera.Follow = target;
            camera.LookAt = target;


            realPriority = camera.Priority;

            CinemachineTransposer transposer = camera.AddCinemachineComponent<CinemachineTransposer>();

            transposer.m_FollowOffset = new Vector3(-9f, 5f, 0f);
            camera.Priority = 3;
        }

        public void ResetGoalCameraPriority()
        {
            _camera.Priority = realPriority;
            PlayerStateContext.canPlayersAct = true;
        }

        /// <summary>
        /// i == 0(Platform -> Chest), i == 1 (Chest -> Platform)
        /// </summary>
        public void ChangeActiveObject(int i)
        {
            if (i == 0)
            {
                _selectedPlatform.ActivateMeshRenderer(false);
                _currentGoal.SetActive(true);
                isGoalActive = true;
            }
            else
            {
                _currentGoal.SetActive(false);
                _selectedPlatform.ActivateMeshRenderer(true);
                isGoalActive = false;
                RandomGoalSelect();
            }
        }

        public void TakeGoblet()
        {
            _chestAnimator = _currentGoal.GetComponent<GoalChestAnimation>();
            _chestAnimator.StartChestOpeningAnimation();
        }

        private void CreateGoalObject(Platform platform)
        {
            _selectedPlatform = platform;
            _currentGoal = Instantiate(GameManager.GoalObject.goalChestObject);
            _currentGoal.SetActive(false);
            _currentGoal.GetComponent<GoalChestAnimation>().GoalSelector = this;
            _currentGoal.transform.position = platform.position;
            SetChestRotation(platform, _currentGoal.transform);
            _platformChangerObject.transform.position = platform.position;
            IEnumerator ChangeCameraAfterBlendCoroutine()
            {
                yield return null; yield return null;
                while (_cinemachineBrain.IsBlending) { yield return null; }
                _platformChangerObject.SetActive(true);
                StopCoroutine(ChangeCameraAfterBlendCoroutine());
            }
            StartCoroutine(ChangeCameraAfterBlendCoroutine());
        }

        private void SetChestRotation(Platform selectedPlatform, Transform chest)
        {
            switch (selectedPlatform.goalSpec)
            {
                case PlatformGoalSpec.up:
                    chest.Rotate(0, -90, 0);
                    break;
                case PlatformGoalSpec.rightup:
                    chest.Rotate(0, -45, 0);
                    break;
                case PlatformGoalSpec.right:
                    chest.Rotate(0, 0, 0);
                    break;
                case PlatformGoalSpec.rightdown:
                    chest.Rotate(0, 45, 0);
                    break;
                case PlatformGoalSpec.down:
                    chest.Rotate(0, 90, 0);
                    break;
                case PlatformGoalSpec.leftdown:
                    chest.Rotate(0, 135, 0);
                    break;
                case PlatformGoalSpec.left:
                    chest.Rotate(0, 180, 0);
                    break;
                case PlatformGoalSpec.leftup:
                    chest.Rotate(0, -135, 0);
                    break;
                default:
                    break;
            }
        }
    }
}