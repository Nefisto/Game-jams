using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyIntEvent : UnityEvent<int> {}

public class GameEventListenerInt : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEventInt Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public MyIntEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int value)
    {
        Response.Invoke(value);
    }
}