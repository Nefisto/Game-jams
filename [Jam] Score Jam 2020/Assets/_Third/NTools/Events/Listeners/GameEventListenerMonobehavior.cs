
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyMonobehaviorEvent : UnityEvent<MonoBehaviour> { }

public class GameEventListenerMonobehavior : MonoBehaviour
{
	[Tooltip("Event to register with.")]
	public GameEventMonoBehavior Event;

	[Tooltip("Response to invoke when Event is raised.")]
	public MyMonobehaviorEvent Response;

	private void OnEnable()
	{
		Event.RegisterListener(this);
	}

	private void OnDisable()
	{
		Event.UnregisterListener(this);
	}

	public void OnEventRaised(MonoBehaviour value)
	{
		Response.Invoke(value);
	}
}