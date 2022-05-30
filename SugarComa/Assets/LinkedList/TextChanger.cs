using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LinkedList
{
    
    public abstract class Observer
    {
        public abstract void OnNotify();
    }

    public class TextChanger : Observer
    {
        public TMP_Text playerGold, playerHealth;
        public Player player;

        public TextChanger(TMP_Text playerGold, TMP_Text playerHealth, Player player)
        {
            this.playerGold = playerGold;
            this.playerHealth = playerHealth;
            this.player = player;
        }

        void ChangeText()
        {
            playerGold.text = "Gold: " + player.gold.ToString();
            playerHealth.text = "Health: " + player.health.ToString();
        }
        public override void OnNotify()
        {
            ChangeText();
        }

    }
}