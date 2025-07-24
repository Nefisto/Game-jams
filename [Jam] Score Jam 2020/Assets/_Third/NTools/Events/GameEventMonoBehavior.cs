
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventMonobehavior", menuName = "Framework/Events/GameEvent(Monobehavior)", order = 0)]
public class GameEventMonoBehavior : ScriptableObject
{
	private List<GameEventListenerMonobehavior> eventListeners
		= new List<GameEventListenerMonobehavior>();

	[SerializeField]
	[Multiline]
	private string DeveloperDescription;

	[Header("Debug")]
	public MonoBehaviour value;

	[Header("Happen every trigger")]
	public MyMonobehaviorEvent defaultBehavior;

	private void OnEnable()
	{
		eventListeners.Clear();
	}

	private void OnDisable()
	{
		eventListeners.Clear();
	}

	public void Raise(MonoBehaviour _value)
	{
		if (defaultBehavior != null)
			defaultBehavior.Invoke(_value);

		for (int i = eventListeners.Count - 1; i >= 0; i--)
			eventListeners[i].OnEventRaised(_value);
	}

	public void RegisterListener(GameEventListenerMonobehavior listener)
	{
		if (!eventListeners.Contains(listener))
			eventListeners.Add(listener);
	}

	public void UnregisterListener(GameEventListenerMonobehavior listener)
	{
		if (eventListeners.Contains(listener))
			eventListeners.Remove(listener);
	}

	public List<GameEventListenerMonobehavior> GetListeners()
		=> eventListeners;
}