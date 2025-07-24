using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DropTableItem))]
public class DropTableItemDrawer : PropertyDrawer
{
    private int numberOfLines = 1;

    private SerializedProperty prefab;
    private SerializedProperty weight;
    private SerializedProperty percent;
    private SerializedProperty isMultiple;
    private SerializedProperty amount;
    private SerializedProperty isGuaranted;
    private SerializedProperty lastWeight;

    // Positions
    private Rect poolRect, weightRect, percentRect, isMultipleRect, isGuarantedRect;
    private Rect secondLineRect;

    private bool canChangeWeight;

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        #region Control variables

        var currentPosition = rect.position;

        var oneLineRect = new Rect(rect.position, new Vector2(rect.width, EditorGUIUtility.singleLineHeight));
        var twoHalfPercentSpaceWidth = oneLineRect.ResizeX(.025f).x;
        
        var originalColor = GUI.color;
        var lowGrayColor = new Color(.9f, .9f, .9f, 1);

        #endregion

        #region Configuring Rects

        // 45% para o pool
        poolRect = new Rect(currentPosition, oneLineRect.ResizeX(.45f));
        currentPosition.x += poolRect.width;

        // 2.5% de espaco
        currentPosition.x += twoHalfPercentSpaceWidth;

        // 10% para o weight
        weightRect = new Rect(currentPosition, oneLineRect.ResizeX(.1f));
        currentPosition.x += weightRect.width;

        // 2.5% de espaco
        currentPosition.x += twoHalfPercentSpaceWidth;

        // 20% para a porcentagem
        percentRect = new Rect(currentPosition, oneLineRect.ResizeX(.2f));
        currentPosition.x += percentRect.width;

        // 10% para o bool do multiple
        isMultipleRect = new Rect(currentPosition, oneLineRect.ResizeX(.1f));
        currentPosition.x += isMultipleRect.width;

        // 10% para o bool do guaranted
        isGuarantedRect = new Rect(currentPosition, oneLineRect.ResizeX(.1f));

        // To amount
        secondLineRect = new Rect(new Vector2(oneLineRect.position.x, oneLineRect.position.y + oneLineRect.height),
                                  new Vector2(oneLineRect.size.x, oneLineRect.size.y * 2));
        secondLineRect.xMin += twoHalfPercentSpaceWidth * 2;
        #endregion

        #region Get fields
            
        prefab = property.FindPropertyRelative("prefab");
        weight = property.FindPropertyRelative("weight");
        percent = property.FindPropertyRelative("percent");
        isMultiple = property.FindPropertyRelative("isMultiple");
        amount = property.FindPropertyRelative("amount");
        isGuaranted = property.FindPropertyRelative("isGuaranted");
        lastWeight = property.FindPropertyRelative("lastWeight");

        #endregion
        
        canChangeWeight = isGuaranted.boolValue;

        #region Draw
        
        EditorGUI.PropertyField(poolRect, prefab, GUIContent.none);

        EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(canChangeWeight);
                EditorGUI.PropertyField(weightRect, weight, GUIContent.none);
            EditorGUI.EndDisabledGroup();
        if (EditorGUI.EndChangeCheck())
        {
            if (weight.intValue < 0)
                weight.intValue = 0;
        }

        GUI.backgroundColor = lowGrayColor;
        var percentMessage = "";
        if (percent.floatValue < 0f)
            percentMessage = "--.--";
        else
            percentMessage = percent.floatValue.ToString("P");
        GUI.Box(percentRect, percentMessage);
        GUI.backgroundColor = originalColor;

        EditorGUI.PropertyField(isMultipleRect.CenterCheckBox(), isMultiple, GUIContent.none);

        if (isMultiple.boolValue)
        {
            numberOfLines = 3;

            secondLineRect.y += 3;

            EditorGUI.PropertyField(secondLineRect, amount);
        }
        else
            numberOfLines = 1;

        EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(isGuarantedRect.CenterCheckBox(), isGuaranted, GUIContent.none);
        if (EditorGUI.EndChangeCheck())
        {
            if (isGuaranted.boolValue)
            {
                lastWeight.intValue = weight.intValue;
                weight.intValue = 0;
            }
            else
                weight.intValue = lastWeight.intValue;
        }

        #endregion
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20f * numberOfLines;
    }
}