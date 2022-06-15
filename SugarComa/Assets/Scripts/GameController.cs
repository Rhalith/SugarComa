using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public PlayerObjects playerObjects;

    readonly NotifyScript notifyScript = new();

    private void Start()
    {
        TextChanger textChanger = new(playerObjects.playerGold, playerObjects.playerHealth,playerObjects.playerGoblet, playerObjects.playerCollector);
        notifyScript.AddObserver(textChanger);
    }

    public void ChangeText()
    {
        notifyScript.Notify();
    }
}

[System.Serializable]
public class PlayerObjects
{
    public PlayerCollector playerCollector;
    public TMP_Text playerGold, playerHealth, playerGoblet;
}
