#pragma warning disable 0414 // Assigned but never used (DeveloperDescription)
#pragma warning disable 0649 // Never assigned (eventListeners)

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventBool", menuName = "Framework/Events/GameEvent(bool)")]
public class GameEventBool : ScriptableObject
{
    private List<GameEventListenerBool> eventListeners
        = new List<GameEventListenerBool>();

    [SerializeField]
    [Multiline]
    private string DeveloperDescription = "";

    [Header("Debug")]
    public bool value;

    [Header("Happen every trigger")]
    public MyBoolEvent defaultBehavior;

    private void OnEnable()
    {
        eventListeners.Clear();
    }

    private void OnDisable()
    {
        eventListeners.Clear();
    }

    public void Raise(bool _value)
    {
        if (defaultBehavior != null)
            defaultBehavior.Invoke(_value);

        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(_value);
    }

    public void RegisterListener(GameEventListenerBool listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerBool listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }

    public List<GameEventListenerBool> GetListeners()
        => eventListeners;
}