using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyFloatEvent : UnityEvent<float> {}

public class GameEventListenerFloat : MonoBehaviour
{
	[Tooltip("Event to register with.")]
	public GameEventFloat Event;

	[Tooltip("Response to invoke when Event is raised.")]
	public MyFloatEvent Response;

	private void OnEnable()
	{
		Event.RegisterListener(this);
	}

	private void OnDisable()
	{
		Event.UnregisterListener(this);
	}

	public void OnEventRaised(float value)
	{
		Response.Invoke(value);
	}
}