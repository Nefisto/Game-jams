using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyBoolEvent : UnityEvent<bool> {}

public class GameEventListenerBool : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEventBool Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public MyBoolEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(bool value)
    {
        Response.Invoke(value);
    }
}