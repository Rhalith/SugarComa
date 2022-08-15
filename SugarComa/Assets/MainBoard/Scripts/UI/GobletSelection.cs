using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Route;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MainBoard.Scripts.UI
{
    public class GobletSelection : MonoBehaviour
    {
        #region Serialize Field

        [SerializeField] private GameController _gameController;
        [SerializeField] private PlayerCollector player;
        [SerializeField] private Button Goblet;
        [SerializeField] private GoalSelector _goalSelector;
        [SerializeField] private PathFinder _pathFinder;
        #endregion

        #region Properties

        public GameController GameController { set { _gameController = value; } }
        public GoalSelector GoalSelector { set { _goalSelector = value; } }
        public PathFinder PathFinder { set { _pathFinder = value; } }
        #endregion

        #region Events

        public delegate void GobletAction();
        public event GobletAction OnTakeIt;
        public event GobletAction OnLeaveIt;
        #endregion

        public void OpenGobletSelection()
        {
            if (player.gold >= 50)
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
            OnTakeIt?.Invoke();
            _goalSelector.TakeGoblet();
        }

        public void LeaveIt()
        {
            _gameController.ChangeText();
            gameObject.SetActive(false);
            OnLeaveIt?.Invoke();
        }
    }
}