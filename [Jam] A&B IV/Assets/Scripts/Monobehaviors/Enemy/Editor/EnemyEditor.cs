using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public Enemy _target
    {
        get => (Enemy)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Follow path"))
            _target.StartFollow();

        if (GUILayout.Button("Die"))
            _target.TakeDamage(10000);
    }
}