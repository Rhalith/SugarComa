using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Utils
{
    [System.Serializable]
    public class FlameMaterials
    {
        public Material[] classic;
        public Material[] explosion;
        public Material[] poison;
        public Material[] sticky;
    }

    [System.Serializable]
    public class EffectMaterials
    {
        public Material[] classic;
        public Material[] explosion;
    }
}