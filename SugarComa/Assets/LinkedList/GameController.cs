using System.Collections;
using System.Collections.Generic;
using UnityEngine; using UnityEngine.UI;
using TMPro;

    public class GameController : MonoBehaviour
    {
        public PlayerObjects playerObjects;

        NotifyScript notifyScript = new NotifyScript();

        private void Start()
        {
            TextChanger textChanger = new TextChanger(playerObjects.playerGold, playerObjects.playerHealth, playerObjects.player);
            notifyScript.AddObserver(textChanger, notifyScript.userInterfaceElements);
        }

        public void ChangeText()
        {
            notifyScript.Notify();
        }
    }
    [System.Serializable]
    public class PlayerObjects
    {
        public Player player;
        public TMP_Text playerGold, playerHealth;
    }



