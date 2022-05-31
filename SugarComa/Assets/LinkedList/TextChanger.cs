using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Observer
{
    public abstract void OnNotify();
}

public class TextChanger : Observer
{
    public TMP_Text playerGold, playerHealth, playerGoblet;
    public Player player;

    public TextChanger(TMP_Text playerGold, TMP_Text playerHealth,TMP_Text playerGoblet, Player player)
    {
        this.playerGold = playerGold;
        this.playerHealth = playerHealth;
        this.playerGoblet = playerGoblet;
        this.player = player;
    }

    void ChangeText()
    {
        playerGold.text = "Gold: " + player.gold.ToString();
        playerHealth.text = "Health: " + player.health.ToString();
        playerGoblet.text = "Goblet: " + player.goblet.ToString();
    }
    public override void OnNotify()
    {
        ChangeText();
    }

}
