using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets.MainBoard.Scripts.GameManaging;

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

    public void RandomGoalSelect()
    {
        int i = Random.Range(0, platforms.Count);
        if (platforms[i].spec != PlatformSpec.Goal && !platforms[i].HasSelector && platforms[i] != _currentPlatform && !platforms[i].isPlayerInPlatform)
        {
            platforms[i].spec = PlatformSpec.Goal;
            CreateGoalObject(platforms[i]);
            VirtualCameraLookTo(_camera, platforms[i].transform);
            print(platforms[i]);
            isAnyGoalPlatform = true;
            PlayerInput.canPlayersAct = false;
            _currentPlatform = platforms[i];
            return;
        }
        else
        {
            RandomGoalSelect();
        }
    }

    public void SelectGoalOnStart()
    {
        int i = Random.Range(0, platforms.Count);
        if (platforms[i].spec != PlatformSpec.Goal && !platforms[i].HasSelector && !platforms[i].isPlayerInPlatform)
        {
            platforms[i].spec = PlatformSpec.Goal;
            CreateGoalObject(platforms[i]);
            VirtualCameraLookTo(_camera, platforms[i].transform);
            PlayerInput.canPlayersAct = false;
            print(platforms[i]);
            isAnyGoalPlatform = true;
            _currentPlatform = platforms[i];
            return;
        }
        else
        {
            SelectGoalOnStart();
        }
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
        PlayerInput.canPlayersAct = true;
    }

    /// <summary>
    /// i == 0(Platform -> Chest), i == 1 (Chest -> Platform)
    /// </summary>
    public void ChangeActiveObject(int i)
    {
        if(i == 0)
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
