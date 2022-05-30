using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine; using UnityEngine.UI;
using TMPro;
namespace LinkedList
{
    public class GameController : MonoBehaviour
    {
        public PlayerObjects playerObjects;

        NotifyScript notifyScript = new NotifyScript();

        private void Start()
        {
            TextChanger textChanger = new TextChanger(playerObjects.playerGold, playerObjects.playerHealth, playerObjects.player);
            notifyScript.AddObserver(textChanger);
        }

        public void ChangeText()
        {
            notifyScript.Notify();
        }
    }
    [Serializable]
    public class PlayerObjects
    {
        public Player player;
        public TMP_Text playerGold, playerHealth;
    }
}


