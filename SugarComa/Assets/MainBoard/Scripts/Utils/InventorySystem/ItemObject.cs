using Assets.MainBoard.Scripts.Player.Items;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Assets.MainBoard.Scripts.Utils.InventorySystem
{
    public class ItemObject : MonoBehaviour
    {
        public InventoryItemData referenceItem;

        [SerializeField] TMP_Text text;

        [SerializeField] PlayerInventory playerInventory;

        public Button _button;
        public void OnAddItem()
        {
            playerInventory.AddItem(referenceItem);
        }
        public void RemoveItem()
        {
            playerInventory.RemoveItem(referenceItem);
        }

        public void NotifyInventory()
        {
            ChangeInventory(playerInventory.GetItem(referenceItem));
        }
        public int CheckItemCount()
        {
            if (playerInventory.GetItem(referenceItem) == null)
            {
                return 0;
            }
            return playerInventory.GetItem(referenceItem).stackSize;
        }

        public void ChangeInventory(InventoryItem item)
        {
            if (item != null)
            {
                text.text = item.stackSize.ToString();
            }
            else
            {
                text.text = 0.ToString();
            }
            gameObject.GetComponent<Image>().sprite = referenceItem.icon;
        }
    }
}