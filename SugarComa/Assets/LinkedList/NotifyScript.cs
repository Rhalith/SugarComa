using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinkedList
{
public class NotifyScript
{
     List<Observer> observers = new List<Observer>();

    public void Notify()
    {
        foreach (Observer observer in observers)
        {
            observer.OnNotify();
        }
    }
    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}
}

