using UnityEngine;
using UnityEngine.UI;

public class GobletSelection : MonoBehaviour
{
    public GameController controller;
    public PlayerCollector player;
    public Button Goblet;
    
    public void OpenGobletSelection()
    {
        if(player.gold >= 50)
        {
            Goblet.interactable = true;
        }
        else
        {
            Goblet.interactable = false;
        }
        gameObject.SetActive(true);
    }
    
    public void Takeit()
    {
        player.goblet++;
        player.gold -= 50;
        controller.ChangeText();
        gameObject.SetActive(false);
    }

    public void Leaveit()
    {
        controller.ChangeText();
        gameObject.SetActive(false);
    }
}
