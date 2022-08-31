using Assets.MainBoard.Scripts.UI;

namespace Assets.MainBoard.Scripts.Utils.InventorySystem
{
    public class InventoryChanger : IObserver
    {
        public ItemObject _item;

        public InventoryChanger(ItemObject item)
        {
            _item = item;
        }

        public void OnNotify()
        {
            _item.NotifyInventory();
        }
    }
}