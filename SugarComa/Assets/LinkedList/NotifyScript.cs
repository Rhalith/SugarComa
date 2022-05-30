using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NotifyScript
{
    public List<Observer> userInterfaceElements = new List<Observer>();

    public void Notify()
    {
        foreach (Observer observer in userInterfaceElements)
        {
            observer.OnNotify();
        }
    }
    public void AddObserver(Observer observer, List<Observer> list)
    {
        userInterfaceElements.Add(observer);
    }
    public void RemoveObserver(Observer observer, List<Observer> list)
    {
        userInterfaceElements.Remove(observer);
    }
}


