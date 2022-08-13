using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Utils
{
    [System.Serializable]
    public class MeteorMeshes
    {
        public Mesh classic;
        public Mesh explosion;
        public Mesh poison;
        public Mesh sticky;
    }
    [System.Serializable]
    public class MeteorMaterials
    {
        public Material[] classic;
        public Material[] explosion;
        public Material[] poision;
        public Material[] sticky;
    }
    [System.Serializable]
    public class MeteorEffectMeshes
    {
        public Mesh classic;
        public Mesh explosion;
        public Mesh poison;
        public Mesh sticky;
    }
}