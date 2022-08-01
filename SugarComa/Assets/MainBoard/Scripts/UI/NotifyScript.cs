using System.Collections.Generic;

namespace Assets.MainBoard.Scripts.UI
{
    public class NotifyScript
    {
        public List<IObserver> userInterfaceElements = new List<IObserver>();

        public List<IObserver> userInventoryElements = new List<IObserver>();

        public void Notify()
        {
            foreach (IObserver observer in userInterfaceElements)
            {
                observer.OnNotify();
            }
        }

        public void NotifyInventory()
        {
            foreach (IObserver observer in userInventoryElements)
            {
                observer.OnNotify();
            }
        }

        public void AddObserver(IObserver observer)
        {
            userInterfaceElements.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            userInterfaceElements.Remove(observer);
        }

        public void AddInventoryObserver(IObserver observer)
        {
            userInventoryElements.Add(observer);
        }

        public void RemoveInventoryObserver(IObserver observer)
        {
            userInventoryElements.Remove(observer);
        }
    }
}