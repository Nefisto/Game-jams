using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System;
using System.Linq;

[CustomEditor(typeof(DropTable))]
public class DropTableEditor : Editor
{
    // FRAME-TODO: Usar GUI.BACKGROUND para alternar a cor dos items
    private SerializedProperty _property;
    private ReorderableList _list;


    // FRAME-TODO: Passar para o snippet
    private DropTable Target
    {
        get => (DropTable)target;
    }

    private void OnEnable()
    {
        
        _list = CreateList(serializedObject, serializedObject.FindProperty("loot"));
    }

    ReorderableList CreateList(SerializedObject obj, SerializedProperty prop)
    {
        ReorderableList list = new ReorderableList(serializedObject, prop, true, true, true, true);

        // https://pastebin.com/WhfRgcdC 
        // Control reorderablelist element size
        List<float> heights = new List<float>();

        list.drawHeaderCallback = (rect) =>
        {
            #region Control

            var originalColor = GUI.color;    
            var lowGray = new Color(.85f, .85f, .85f, 1);

            rect.xMin += 16; // constante drag size in reordarable list
            
            var currentPosition = rect.position;
            var twoHalfSpace = rect.ResizeX(.025f).x;

            #endregion

            #region Rects

            var nameRect = new Rect(currentPosition, rect.ResizeX(.45f));
            currentPosition.x += nameRect.width;

            currentPosition.x += twoHalfSpace;

            var weightRect = new Rect(currentPosition, rect.ResizeX(.1f));
            currentPosition.x += weightRect.width;

            currentPosition.x += twoHalfSpace;

            var percentRect = new Rect(currentPosition, rect.ResizeX(.2f));
            currentPosition.x += percentRect.width;

            var isMultipleRect = new Rect(currentPosition, rect.ResizeX(.1f));
            currentPosition.x += isMultipleRect.width;

            var isGuarantedRect = new Rect(currentPosition, rect.ResizeX(.1f));
            
            #endregion

            #region Draw
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                        
            GUI.Label(nameRect, "Prefab", new GUIStyle(){alignment = TextAnchor.MiddleLeft});

            GUI.color = lowGray;
            GUI.Box(weightRect, "");
            GUI.Label(weightRect, new GUIContent("W"){tooltip = "weight"});

            GUI.color = Color.white;
            GUI.Label(percentRect, new GUIContent("%"));

            GUI.color = lowGray;
            GUI.Box(isMultipleRect, "");
            GUI.Label(isMultipleRect, new GUIContent("M"){tooltip = "drop multiple itens?"});

            GUI.color = Color.white;
            GUI.Label(isGuarantedRect, new GUIContent("G"){tooltip = "is guaranted drop?"});

            #endregion
        };

        list.drawElementCallback = (rect, index, active, focused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            float height = EditorGUIUtility.singleLineHeight * 1.25f;
            if (element.FindPropertyRelative("isMultiple").boolValue)
                height = EditorGUIUtility.singleLineHeight * 3.5f;

            try
            {
                heights[index] = height;
            }
            catch (ArgumentOutOfRangeException)
            {
                // FRAME-TODO: Arrumar esse supress
                // Ele entra com index = 0 quando a quantia de elemento é 0...
                // Debug.LogWarning(e.Message); 
            }
            finally
            {
                float[] floats = heights.ToArray();
                Array.Resize(ref floats, prop.arraySize);
                heights = floats.ToList();
            }

            float margin = height * .05f;
            rect.y += margin;

            EditorGUI.PropertyField(rect, element);
        };

        list.elementHeightCallback = (index) =>
        {
            Repaint();
            float height = 0;

            try
            {
                height = heights[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                // FRAME-TODO: Arrumar esse supress
                // Ele entra com index = 0 quando a quantia de elemento é 0...
                // Debug.LogWarning(e.Message); 
            }
            finally
            {
                float[] floats = heights.ToArray();
                Array.Resize(ref floats, prop.arraySize);
                heights = floats.ToList();
            }

            return height;
        };

        list.onAddCallback = (_list) =>
        {
            var lastPos = _list.serializedProperty.arraySize;
            _list.serializedProperty.InsertArrayElementAtIndex(lastPos);
            
            var element = _list.serializedProperty.GetArrayElementAtIndex(lastPos);
            
            element.FindPropertyRelative("prefab").objectReferenceValue = null;
            element.FindPropertyRelative("weight").intValue = default(int);
            element.FindPropertyRelative("percent").floatValue = default(float);
            element.FindPropertyRelative("isMultiple").boolValue = default(bool);
            element.FindPropertyRelative("isGuaranted").boolValue = default(bool);

            var elementAmount = element.FindPropertyRelative("amount");

            elementAmount.FindPropertyRelative("min").intValue = default(int);
            elementAmount.FindPropertyRelative("max").intValue = default(int);
            elementAmount.FindPropertyRelative("currentMin").intValue = default(int);
            elementAmount.FindPropertyRelative("currentMax").intValue = default(int);
        };

        return list;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Target.CalculatePercent();

        _list.DoLayoutList();
        
        if (GUILayout.Button("Clear list"))
            Target.loot.Clear();

        serializedObject.ApplyModifiedProperties();
    }
}