using UnityEngine;
using UnityEngine.UI;

public class GobletSelection : MonoBehaviour
{
    [SerializeField] GameController _gameController;
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
        _gameController.ChangeText();
        gameObject.SetActive(false);
        _playerMovement._current.ResetSpec();
        _goalSelector.RandomGoalSelect();
    }

    public void LeaveIt()
    {
        _gameController.ChangeText();
        gameObject.SetActive(false);
    }
    public void SetGameController(GameController gameController)
    {
        _gameController = gameController;
    }

    public void SetGoalSelector(GoalSelector goalSelector)
    {
        _goalSelector = goalSelector;
    }
}
