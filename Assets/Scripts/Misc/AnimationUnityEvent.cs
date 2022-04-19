using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationUnityEvent : MonoBehaviour
{
    public UDictionary<string, UnityEvent> events;

    public void CallEvent(string eventName)
    {
        if(events.ContainsKey(eventName))
            events[eventName].Invoke();
    }
}
