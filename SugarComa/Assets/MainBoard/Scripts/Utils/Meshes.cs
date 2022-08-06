using UnityEngine;

namespace Assets.MainBoard.Scripts.Utils
{
    [System.Serializable]
    public class GoldMeshes
    {
        public Mesh goldMesh1;
        public Mesh goldMesh2;
        public Mesh goldMesh3;
    }

    [System.Serializable]
    public class HealMeshes
    {
        public Mesh healMesh1;
        public Mesh healMesh2;
    }

    [System.Serializable]
    public class RandomBoxMeshes
    {
        public Mesh randomBoxMesh1;
        public Mesh randomBoxMesh2;
        public Mesh randomBoxMesh3;
    }

    [System.Serializable]
    public class TrapMeshes
    {
        public Mesh trapMesh1;
        public Mesh trapMesh2;
        public Mesh trapMesh3;
    }

    [System.Serializable]
    public class JackpotMeshes
    {
        public Mesh jackpotMesh;
    }

    [System.Serializable]

    public class GoalObject
    {
        public GameObject goalChestObject;
        //public GameObject goalAnimationObject; not necessary right now
    }
}