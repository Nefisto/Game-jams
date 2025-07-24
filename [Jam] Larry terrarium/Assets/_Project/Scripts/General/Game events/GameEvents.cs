using System;

public static class GameEvents
{
    public static Action OnGameStart { get; set; }
    public static Action OnUnlockPlayerMovement { get; set; }
}