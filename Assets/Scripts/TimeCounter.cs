using System;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    public static TimeCounter instance;
    
    private DateTime dateEntry;
    private DateTime dateExit;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    public void SaveEntry()
    {
        dateEntry = DateTime.Now;
    }

    public void SaveTime()
    {
        dateExit = DateTime.Now;
        Nucleus.instance.AddVisit(dateEntry, dateExit);
    }
}
