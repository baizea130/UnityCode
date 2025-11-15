using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventCenter
{
    public static Dictionary<string, Action<object>> eventDict = new Dictionary<string, Action<object>>();
    public static void AddListener(string name, Action<object> action)
    {
        if (!eventDict.ContainsKey(name))
        {
            eventDict[name] = null;
        }
        eventDict[name] += action;
    }
    public static void RemoveListener(string name, Action<object> action)
    {
        if (eventDict.ContainsKey(name))
        {
            eventDict[name] -= action;
        }
    }
    public static void TriggerEvent(string name,object param = null)
    {
        if (eventDict[name] != null)
        {
            eventDict[name]?.Invoke(param);
        }
    }
    public static void DebugEventCenterName()
    {
        Debug.Log("=======================");
        for (int i = 0; i < eventDict.Count; i++)
        {
            var key = eventDict.Keys.ElementAt(i);
            Debug.Log(i+".key:"+key);
        }
        Debug.Log("=======================");
    }
}
