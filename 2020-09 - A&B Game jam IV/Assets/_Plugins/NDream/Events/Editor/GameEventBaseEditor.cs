using UnityEngine;
using UnityEditor;

public abstract class GameEventBaseEditor<T> : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        T e = GetTarget();
        
        if (GUILayout.Button("Raise"))
            Raise(e);
    }

    protected abstract T GetTarget();
    protected abstract void Raise(T eventToRaise);
}