using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEventInt))]
public class GameEventIntEditor : GameEventBaseEditor<GameEventInt>
{   
    protected override GameEventInt GetTarget()
        => target as GameEventInt;

    protected override void Raise(GameEventInt eventToRaise)
        => eventToRaise.Raise(eventToRaise.value);
}