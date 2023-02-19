using Assets.MainBoard.Scripts.Utils.PlatformUtils;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.Player.States;
using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Networking;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Cinemachine;
using Assets.MainBoard.Scripts.Networking.MainBoardNetworking;

namespace Assets.MainBoard.Scripts.Route
{
    public class GoalSelector : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] List<Platform> platforms;

        [SerializeField] CinemachineVirtualCamera _camera;

        [SerializeField] CinemachineBrain _cinemachineBrain;
        #endregion

        #region Private Fields
        private int realPriority;

        private Platform _selectedPlatform;

        private GameObject _currentGoal;

        private Platform _currentPlatform;
        #endregion

        #region Public Fields
        public GameObject PlatformChangerObject;

        public GoalChestAnimation ChestAnimator;

        public bool isGoalActive;

        /// <summary>
        /// if there is a goal platform in map
        /// </summary>
        public static bool isAnyGoalPlatform;

        public bool isChestAnim;
        #endregion

        int imd = 3;
        public void RandomGoalSelect()
        {
            //int index = Random.Range(1, platforms.Count);

            int index = imd;
            imd++;

            if (platforms[index].spec != PlatformSpec.Goal && !platforms[index].HasSelector)
            {
                // Checks for not placing chest same platform again.
                if(_currentPlatform != null && platforms[index] == _currentPlatform)
                {
                    RandomGoalSelect();
                }
                else
                {
                    bool result = RemoteMessageHandler.Instance.SendNewChestIndex(index);

                    if (result)
                        StartCoroutine(CreateGoal(index));
                }
            }
            else
            {
                RandomGoalSelect();
            }
        }

        public IEnumerator CreateGoal(int index)
        {
            if (isAnyGoalPlatform)
            {
                _currentPlatform.ResetSpec();

                //isChestAnim become true inside here
                TakeGoblet();
            }

            while (isChestAnim)
            {

            }

            platforms[index].spec = PlatformSpec.Goal;
            CreateGoalObject(platforms[index]);
            VirtualCameraLookTo(_camera, platforms[index].transform);
            print(platforms[index]);
            isAnyGoalPlatform = true;
            _currentPlatform = platforms[index];
            yield return null;
        }

        /*
        public void CreateGoalMeth(int index)
        {
            if (isAnyGoalPlatform)
            {
                _currentPlatform.ResetSpec();

                //isChestAnim become true inside here
                TakeGoblet();
            }

            /* use an asyncronous method for waiting. 
            while (isChestAnim)
            {
            }
            //
            platforms[index].spec = PlatformSpec.Goal;
            CreateGoalObject(platforms[index]);
            VirtualCameraLookTo(_camera, platforms[index].transform);
            print(platforms[index]);
            isAnyGoalPlatform = true;
            PlayerStateContext.canPlayersAct = false;
            _currentPlatform = platforms[index];
             
        }
        */

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
            ChestAnimator = _currentGoal.GetComponent<GoalChestAnimation>();
            ChestAnimator.StartChestOpeningAnimation();
        }

        private void CreateGoalObject(Platform platform)
        {
            _selectedPlatform = platform;
            _currentGoal = Instantiate(GameManager.GoalObject.goalChestObject);
            _currentGoal.SetActive(false);
            _currentGoal.GetComponent<GoalChestAnimation>().GoalSelector = this;
            _currentGoal.transform.position = platform.position;

            SetChestRotation(platform, _currentGoal.transform);

            PlatformChangerObject.transform.position = platform.position;

            IEnumerator ChangeCameraAfterBlendCoroutine()
            {
                // why yield return null two times
                yield return null; yield return null;
                while (_cinemachineBrain.IsBlending) { yield return null; }

                // Torus activated here...
                PlatformChangerObject.SetActive(true);
                PlatformChangerObject.GetComponent<Animator>().SetTrigger("PutChest");
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