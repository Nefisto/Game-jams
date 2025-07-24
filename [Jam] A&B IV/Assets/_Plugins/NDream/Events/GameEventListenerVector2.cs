using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyVector2Event : UnityEvent<Vector2> {}

public class GameEventListenerVector2 : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEventVector2 Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public MyVector2Event Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Vector2 value)
    {
        Response.Invoke(value);
    }
}