using UnityEngine;
using UnityEngine.UI;

public class GobletSelection : MonoBehaviour
{
    public GameController controller;
    public PlayerCollector player;
    public Button Goblet;
    [SerializeField] GoalSelector _goalSelector;
    
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
        _goalSelector.RandomGoalSelect();
    }

    public void LeaveIt()
    {
        controller.ChangeText();
        gameObject.SetActive(false);
    }
}
