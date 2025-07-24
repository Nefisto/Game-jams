using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnly))]
public class ReadOnlyDrawer: PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;

        EditorGUI.PropertyField(rect, property, label);

        GUI.enabled = true;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var modifier = 1;
        // ! HACK: por alguma razao usar o (meu) read only com vetores, faz com que eles usem duas linhas no inpector
        // ! mas sem que o layout reconheca
        switch (property.propertyType)
        {
            case SerializedPropertyType.Vector2:
                modifier = 2;
            break;
        }

        return base.GetPropertyHeight(property, label) * modifier;
    }
}
#endif