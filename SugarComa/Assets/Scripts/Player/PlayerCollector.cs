using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public int health;
    public int gold;
    public int goblet;

    [SerializeField] private GameController _gameController;
    [SerializeField] private GobletSelection _gobletSelection;

    public void CheckCurrentNode(Platform platform)
    {
        switch (platform.specification)
        {
            case PlatformSpecification.Gold: AddGold(Random.Range(5, 8)); break;
            case PlatformSpecification.Heal: AddHealth(5); break;
            case PlatformSpecification.Gift: AddItem(); break;
            case PlatformSpecification.Jackpot: RandomJackpot(5); break;
            case PlatformSpecification.Goal: GobletSelection(); break;
        }
    }

    void AddGold(int value)
    {
        gold += value;
        _gameController.ChangeText();
    }
    
    void AddHealth(int value)
    {
        health += value;
        if (health > 30)
        {
            health = 30;
        }
        _gameController.ChangeText();
    }

    void AddItem()
    {
        int i = Random.Range(1, 11);
        switch (i)
        {
            case 1:
                print("Kalkan");
                break;
            case 2:
                print("Kurmali araba");
                break;
            case 3:
                print("Büyük sapan");
                break;
            case 4:
                print("Teleport");
                break;
            case 5:
                print("Ari");
                break;
            case 6:
                print("M?knat?s");
                break;
            case 7:
                print("Sürpriz Kutusu");
                break;
            case 8:
                print("Boks eldiveni");
                break;
            case 9:
                print("Saglik kutusu");
                break;
            case 10:
                print("Kanca");
                break;
            default:
                break;
        }
    }

    void RandomJackpot(int value)
    {
        int i = Random.Range(1, 4);
        switch (i)
        {
            case 1: AddItem(); break;
            case 2: AddGold(Random.Range(value, 8)); break;
            case 3: AddHealth(value); break;
        }
    }

    void GobletSelection()
    {
        _gobletSelection.OpenGobletSelection();
    }
}
