#pragma warning disable 0414 // Assigned but never used (DeveloperDescription)
#pragma warning disable 0649 // Never assigned (eventListeners)

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventfloat", menuName = "Framework/Events/GameEvent(float)")]
public class GameEventFloat : ScriptableObject
{
	private List<GameEventListenerFloat> eventListeners
		= new List<GameEventListenerFloat>();

	[SerializeField]
	[Multiline]
	private string DeveloperDescription = "";

	public float value;

	[Header("Happen every triggered time")]
	public MyFloatEvent defaultBehavior;

	private void OnEnable()
	{
		eventListeners.Clear();
	}

	private void OnDisable()
	{
		eventListeners.Clear();
	}

	public void Raise(float _value)
	{
		defaultBehavior?.Invoke(_value);

		for (int i = eventListeners.Count - 1; i >= 0; i--)
			eventListeners[i].OnEventRaised(_value);
	}

	public void RegisterListener(GameEventListenerFloat listener)
	{
		if (!eventListeners.Contains(listener))
			eventListeners.Add(listener);
	}

	public void UnregisterListener(GameEventListenerFloat listener)
	{
		if (eventListeners.Contains(listener))
			eventListeners.Remove(listener);
	}

	public List<GameEventListenerFloat> GetListeners()
		=> eventListeners;
}