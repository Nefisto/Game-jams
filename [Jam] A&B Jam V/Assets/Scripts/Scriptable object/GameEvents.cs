using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameEvents : ScriptableObject
{
    public static Action OnStartGame;
    public static Action OnError;

    public static Action OnStartCheatMode;
    public static Action OnApplyCheatMode;
    public static Action OnStopCheatMode;
    
    [Button, DisableInEditorMode]
    public static void StartGame()
    {
        OnStartGame?.Invoke();
    }
}