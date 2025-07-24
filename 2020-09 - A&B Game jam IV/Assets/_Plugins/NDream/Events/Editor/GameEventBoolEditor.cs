using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEventBool))]
public class GameEventBoolEditor : GameEventBaseEditor<GameEventBool>
{   
    protected override GameEventBool GetTarget()
        => target as GameEventBool;

    protected override void Raise(GameEventBool eventToRaise)
        => eventToRaise.Raise(eventToRaise.value);
}