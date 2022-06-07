using System.Collections.Generic;

public class NotifyScript
{
    public List<IObserver> userInterfaceElements = new List<IObserver>();

    public void Notify()
    {
        foreach (IObserver observer in userInterfaceElements)
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
}


