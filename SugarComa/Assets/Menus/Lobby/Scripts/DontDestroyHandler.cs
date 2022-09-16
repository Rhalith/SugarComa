using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Menus.Lobby.Scripts
{
    public class DontDestroyHandler : MonoBehaviour
    {
        public GameObject ServerManager;
        public GameObject SteamManager;

        public void DestroyDontDestroys()
        {
            Destroy(SteamManager);
            Destroy(ServerManager);
        }
    }
}