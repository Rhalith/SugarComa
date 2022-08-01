using TMPro;

public interface IObserver
{
    void OnNotify();
}

public class TextChanger : IObserver
{
    public PlayerCollector player;
    public TMP_Text playerGold, playerHealth, playerGoblet;

    public TextChanger(TMP_Text playerGold, TMP_Text playerHealth,TMP_Text playerGoblet, PlayerCollector player)
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

    public void OnNotify()
    {
        ChangeText();
    }
}
