using UnityEngine;
using UnityEngine.UI;

public class GobletSelection : MonoBehaviour
{
    [SerializeField] GameController controller;
    [SerializeField] PlayerCollector player;
    [SerializeField] Button Goblet;
    [SerializeField] GoalSelector _goalSelector;
    [SerializeField] PlayerMovement _playerMovement;

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
        _playerMovement._current.ResetSpec();
        _goalSelector.RandomGoalSelect();
    }

    public void LeaveIt()
    {
        controller.ChangeText();
        gameObject.SetActive(false);
    }
}
