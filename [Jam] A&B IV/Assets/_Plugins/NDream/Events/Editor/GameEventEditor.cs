using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : GameEventBaseEditor<GameEvent>
{
    protected override GameEvent GetTarget()
        => target as GameEvent;

    protected override void Raise(GameEvent eventToRaise)
        => eventToRaise.Raise();
}