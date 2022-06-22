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
        RandomGoalSelect(true);
        RandomGoalSelect(true);
    }

    public void RandomGoalSelect(bool isStart = false)
    {
        int i = Random.Range(0, platforms.Count+1);
        if (platforms[i].specification != PlatformSpecification.Goal)
        {
            platforms[i].specification = PlatformSpecification.Goal;
            if(isStart == false) VirtualCameraLookTo(_camera, platforms[i].transform);
            print(platforms[i]);
            return;
        }
        else
        {
            RandomGoalSelect();
        }
    }

    private void VirtualCameraLookTo(CinemachineVirtualCamera camera, Transform target)
    {
        camera.Follow = target;
        camera.LookAt = target;

         
        realPriority = camera.Priority;

        CinemachineTransposer transposer = camera.AddCinemachineComponent<CinemachineTransposer>();

        transposer.m_FollowOffset = new Vector3(-9f, 5f, 0f);
        camera.Priority = 6;

    }

    public void ResetCameraPriority(CinemachineVirtualCamera camera)
    {
        camera.Priority = realPriority;
    }
}
