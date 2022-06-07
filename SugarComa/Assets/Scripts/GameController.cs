using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public PlayerObjects playerObjects;

    readonly NotifyScript notifyScript = new();

    private void Start()
    {
        var playerCollector = playerObjects.player.GetComponent<PlayerCollector>();

        TextChanger textChanger = new(playerObjects.playerGold, playerObjects.playerHealth,playerObjects.playerGoblet, playerCollector);
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
    public GameObject player;
    public TMP_Text playerGold, playerHealth, playerGoblet;
}
