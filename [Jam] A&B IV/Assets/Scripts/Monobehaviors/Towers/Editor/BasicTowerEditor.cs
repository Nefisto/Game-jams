using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicTower), true)]
public class BasicTowerEditor : Editor
{
    public BasicTower _target
    {
        get => (BasicTower)target;
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.BeginHorizontal();
        
            if (GUILayout.Button("Attack"))
                _target.Attack();

            if (GUILayout.Button("Stop attack"))
                _target.StopAttack();
                
        EditorGUILayout.EndHorizontal();
    }
}