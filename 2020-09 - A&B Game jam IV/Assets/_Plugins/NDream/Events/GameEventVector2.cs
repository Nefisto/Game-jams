#pragma warning disable 0414 // Assigned but never used (DeveloperDescription)
#pragma warning disable 0649 // Never assigned (eventListeners)

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventVector2", menuName = "Framework/Events/GameEvent(Vector 2)")]
public class GameEventVector2 : ScriptableObject
{
    private List<GameEventListenerVector2> eventListeners = 
        new List<GameEventListenerVector2>();


    [SerializeField]
    [Multiline]
    private string DeveloperDescription = "";

    [Header("Debug")]
    public Vector2 value;

    [Header("Happen every trigger")]
    public MyVector2Event defaultBehavior;

    private void OnEnable()
    {
        eventListeners.Clear();
    }

    private void OnDisable()
    {
        eventListeners.Clear();
    }

    public void Raise(Vector2 _value)
    {
        if (defaultBehavior != null)
            defaultBehavior.Invoke(_value);

        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(_value);
    }

    public void RegisterListener(GameEventListenerVector2 listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerVector2 listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}