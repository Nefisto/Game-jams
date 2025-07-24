using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bullet))]
public class BulletEditor : Editor
{
    public Bullet _target
    {
        get => (Bullet)base.target;
    }
    
    private Enemy targetToFollow;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.BeginHorizontal();
        
        targetToFollow = (Enemy)EditorGUILayout.ObjectField(targetToFollow, typeof(Enemy), true);

        GUI.enabled = (targetToFollow != null && Application.isPlaying);

        if (GUILayout.Button("Follow"))
            _target.StartFollowTarget(targetToFollow);
        
        EditorGUILayout.EndHorizontal();
    }
}