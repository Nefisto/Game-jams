using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyTransformEvent : UnityEvent<Transform> {}

public class GameEventListenerTransform : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEventTransform Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public MyTransformEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Transform value)
    {
        Response.Invoke(value);
    }
}