#pragma warning disable 0414 // Assigned but never used (DeveloperDescription)
#pragma warning disable 0649 // Never assigned (eventListeners)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEventTransform", menuName = "Framework/Events/GameEvent(Transform)")]
public class GameEventTransform : ScriptableObject
{
    private List<GameEventListenerTransform> eventListeners = 
        new List<GameEventListenerTransform>();   

    [SerializeField]
    [Multiline]
    private string DeveloperDescription = "";

    private void OnEnable()
    {
        eventListeners.Clear();
    }

    private void OnDisable()
    {
        eventListeners.Clear();
    }

    public void Raise(Transform _value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(_value);
    }

    public void RegisterListener(GameEventListenerTransform listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerTransform listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}