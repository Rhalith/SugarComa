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
        public void SetParent(GameObject parent, int index)
        {
            gameObject.transform.parent = parent.transform;
            ChangePlayerName(playerText, index);
        }

        private void ChangePlayerName(TMP_Text player, int index)
        {
            player.text = "Player " + index;
        }
    }
}