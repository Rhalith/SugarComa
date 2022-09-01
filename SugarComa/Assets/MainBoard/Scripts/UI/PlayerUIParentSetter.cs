using UnityEngine;
using TMPro;

namespace Assets.MainBoard.Scripts.UI
{
    public class PlayerUIParentSetter : MonoBehaviour
    {
        [SerializeField] TMP_Text playerText;

        public GameObject parent;

        private void Start()
        {
            if (parent != null) gameObject.transform.parent = parent.transform;
        }

        public void SetUIParent(GameObject parent, int index, string playerName)
        {
            gameObject.transform.SetParent(parent.transform);
            ChangePlayerName(playerText, playerName);
        }

        private void ChangePlayerName(TMP_Text player, string playerName)
        {
            player.text = playerName;
        }
    }
}