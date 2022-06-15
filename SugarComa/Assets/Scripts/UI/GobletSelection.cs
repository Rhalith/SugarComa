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
    
    public void TakeIt()
    {
        player.goblet++;
        player.gold -= 50;
        controller.ChangeText();
        gameObject.SetActive(false);
    }

    public void LeaveIt()
    {
        controller.ChangeText();
        gameObject.SetActive(false);
    }
}
