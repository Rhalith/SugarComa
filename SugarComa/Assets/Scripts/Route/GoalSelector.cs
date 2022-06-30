using System.Collections;
using System.Collections.Generic;
using UnityEngine; using Cinemachine;

public class GoalSelector : MonoBehaviour
{
    [SerializeField] List<Platform> platforms;

    [SerializeField] CinemachineVirtualCamera _camera;

    [SerializeField] CinemachineBrain _cinemachineBrain;

    private int realPriority;

    private void Start()
    {
        SelectGoalOnStart();
    }

    public void RandomGoalSelect()
    {
        int i = Random.Range(0, platforms.Count);
        if (platforms[i].spec != PlatformSpec.Goal && !platforms[i].HasSelector)
        {
            platforms[i].spec = PlatformSpec.Goal;
            VirtualCameraLookTo(_camera, platforms[i].transform);
            print(platforms[i]);
            return;
        }
        else
        {
            RandomGoalSelect();
        }
    }

    private void SelectGoalOnStart()
    {
        int i = Random.Range(0, platforms.Count);
        if (platforms[i].spec != PlatformSpec.Goal && !platforms[i].HasSelector)
        {
            platforms[i].spec = PlatformSpec.Goal;
            print(platforms[i]);
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
    }
}
