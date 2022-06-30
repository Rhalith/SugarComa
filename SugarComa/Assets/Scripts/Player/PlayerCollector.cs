using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public int health;
    public int gold;
    public int goblet;

    [SerializeField] private GameController _gameController;
    [SerializeField] private GobletSelection _gobletSelection;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] Item item;

    public void CheckCurrentNode(Platform platform)
    {
        switch (platform.specification)
        {
            case PlatformSpec.Gold: AddGold(Random.Range(5, 8)); break;
            case PlatformSpec.Heal: AddHealth(5); break;
            case PlatformSpec.Gift: AddItem(); break;
            case PlatformSpec.Jackpot: RandomJackpot(5); break;
            case PlatformSpec.Goal: GobletSelection(); break;
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

    public void AddItem()
    {
        //int i = Random.Range(1, 11);
        int i = 8;
        switch (i)
        {
            case 1:
                item.shield.OnAddItem();
                break;
            case 2:
                item.car.OnAddItem();
                break;
            case 3:
                item.sapan.OnAddItem();
                break;
            case 4:
                item.teleport.OnAddItem();
                break;
            case 5:
                item.bee.OnAddItem();
                break;
            case 6:
                item.magnet.OnAddItem();
                break;
            case 7:
                item.randombox.OnAddItem();
                break;
            case 8:
                item.boxgloves.OnAddItem();
                break;
            case 9:
                item.healbox.OnAddItem();
                break;
            case 10:
                item.hook.OnAddItem();
                break;
            default:
                break;
        }
        _gameController.ChangeInventory();
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

    public void SetGameController(GameController gameController)
    {
        _gameController = gameController;
    }

    [System.Serializable]
    public class Item
    {
        public ItemObject shield;
        public ItemObject car;
        public ItemObject sapan;
        public ItemObject teleport;
        public ItemObject bee;
        public ItemObject magnet;
        public ItemObject randombox;
        public ItemObject boxgloves;
        public ItemObject healbox;
        public ItemObject hook;
    }
}
