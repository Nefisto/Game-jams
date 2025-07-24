using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

[CustomEditor(typeof(AudioEvent), true)]
public class AudioEventEditor : OdinEditor
{
    [SerializeField] 
    private AudioSource _previewer;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        DestroyImmediate(_previewer.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if (GUILayout.Button("Preview"))
        {
            ((AudioEvent)target).Play(_previewer);
        }
        EditorGUI.EndDisabledGroup();
    }
}
