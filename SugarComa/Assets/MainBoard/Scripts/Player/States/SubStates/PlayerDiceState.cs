using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.States.SubStates
{
    [System.Serializable]
    public class PlayerDiceState : PlayerBaseState
    {
        #region Private Members
        [SerializeField] private GameObject _dice;
        [SerializeField] private TMPro.TMP_Text _diceText;
        #endregion
        public GameObject Dice => _dice;

        public PlayerDiceState(PlayerStateContext context, PlayerData playerData, string animBoolName, bool sendData = true) : base(context, playerData, animBoolName, sendData)
        {
        }

        public override void Enter()
        {
            _dice.SetActive(true);
        }

        public void RollDice()
        {
            int tempStep = Random.Range(1, 10);
            context.Running.CurrentStep = tempStep;
            _diceText.text = tempStep.ToString();
        }

        public override void Exit()
        {
            _dice.SetActive(false);
        }
    }
}
