using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatRange), true)]
public class FloatRangeDrawer : PropertyDrawer
{
    private Rect nameRect, minRect, maxRect, currentMinRect, minMaxSliderRect, currentMaxRect;

    private SerializedProperty minProp, maxProp, currentMinProp, currentMaxProp;

    private float _currentMin, _currentMax;

    // For change check    
    protected float _min, _max;

    private bool isDirty;

    private int numberOfLines = 2;

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        // label = EditorGUI.BeginProperty(rect, label, property);

        #region Control variables

        isDirty = false;

        var currentPosition = rect.position;
        var oneLineRect = new Rect(rect.position, new Vector2(rect.size.x, EditorGUIUtility.singleLineHeight));
        var twoHalfSpaceBox = oneLineRect.ResizeX(.025f).x;

        var originalColor = GUI.color;
        var lowGrayColor = new Color(.8f, .8f, .8f, 1);

        // Have atributes
        var ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof(MinMaxRangeAttribute), true);
        var hasAttribute = ranges.Length > 0;

        #endregion

        #region Creating rects

        // 30% para o nome
        nameRect = new Rect(currentPosition, oneLineRect.ResizeX(.3f));
        currentPosition.x += nameRect.width;

        if (!hasAttribute)
        {
            // 30% para o minValue
            minRect = new Rect(currentPosition, oneLineRect.ResizeX(.3f));
            currentPosition.x += minRect.width;

            // 5% de espaco entre os labels
            currentPosition.x += twoHalfSpaceBox * 4;

            // 35% para o maxValue
            maxRect = new Rect(currentPosition, oneLineRect.ResizeX(.3f));

            // Reset fullRect and jump 1 line
            currentPosition = rect.position;
            currentPosition.y += EditorGUIUtility.singleLineHeight;

            // Left pad
            currentPosition.x += twoHalfSpaceBox * 6;

            // 20% para o currentMinBox
            currentMinRect = new Rect(currentPosition, oneLineRect.ResizeX(.2f));
            currentPosition.x += currentMinRect.width;

            currentPosition.x += twoHalfSpaceBox;

            // 50% para o slider
            minMaxSliderRect = new Rect(currentPosition, oneLineRect.ResizeX(.4f));
            currentPosition.x += minMaxSliderRect.width;

            currentPosition.x += twoHalfSpaceBox;

            // 20% para o currentMaxBox
            currentMaxRect = new Rect(currentPosition, oneLineRect.ResizeX(.2f));
            currentPosition.x += currentMaxRect.width;
        }
        else
        {
            // 20% para o currentMinBox
            currentMinRect = new Rect(currentPosition, oneLineRect.ResizeX(.15f));
            currentPosition.x += currentMinRect.width;

            currentPosition.x += twoHalfSpaceBox;

            // 50% para o slider
            minMaxSliderRect = new Rect(currentPosition, oneLineRect.ResizeX(.35f));
            currentPosition.x += minMaxSliderRect.width;

            currentPosition.x += twoHalfSpaceBox;

            // 20% para o currentMaxBox
            currentMaxRect = new Rect(currentPosition, oneLineRect.ResizeX(.15f));
            currentPosition.x += currentMaxRect.width;
        }

        #endregion

        #region GetProps

        minProp = property.FindPropertyRelative("min");
        maxProp = property.FindPropertyRelative("max");
        currentMinProp = property.FindPropertyRelative("currentMin");
        currentMaxProp = property.FindPropertyRelative("currentMax");

        _min = minProp.floatValue;
        _max = maxProp.floatValue;

        #endregion

        #region Draw

        EditorGUI.LabelField(nameRect, property.displayName);

        // If have attribute
        if (hasAttribute)
        {
            numberOfLines = 1;

            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(minMaxSliderRect, GUIContent.none, ref _currentMin, ref _currentMax, ranges[0].Min, ranges[0].Max);
            if (EditorGUI.EndChangeCheck())
            {
                currentMinProp.floatValue = _currentMin;
                currentMaxProp.floatValue = _currentMax;
            }
        }
        else
        {
            numberOfLines = 2;

            EditorGUIUtility.labelWidth = maxRect.width * .4f;

            EditorGUI.BeginChangeCheck();
            var tmpMin = EditorGUI.FloatField(minRect, "Min", minProp.floatValue);
            var tmpMax = EditorGUI.FloatField(maxRect, "Max", maxProp.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                isDirty = true;
                if (_max != tmpMax)
                    _max = tmpMax;

                if (_min != tmpMin)
                    _min = tmpMin;
            }

            EditorGUIUtility.labelWidth = 0;

            _currentMin = currentMinProp.floatValue;
            _currentMax = currentMaxProp.floatValue;

            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(minMaxSliderRect, GUIContent.none, ref _currentMin, ref _currentMax, minProp.floatValue, maxProp.floatValue);
            if (EditorGUI.EndChangeCheck())
                ValidadeValues(_currentMin, _currentMax);
            
            Event e = Event.current;
            if ((e.keyCode == KeyCode.KeypadEnter || e.keyCode == KeyCode.Escape || e.keyCode == KeyCode.Tab || e.isMouse)
                || (isDirty))
            {
                ValidadeValues(_currentMin, _currentMax);
            }
        }

        GUI.Box(currentMinRect, currentMinProp.floatValue.ToString("F2"));
        GUI.Box(currentMaxRect, currentMaxProp.floatValue.ToString("F2"));

        #endregion

        // EditorGUI.EndProperty();
    }

    protected void ValidadeValues(float currentMin, float currentMax)
    {
        if (_min > _max)
            _max = _min;

        currentMin = Mathf.Clamp(currentMin, _min, currentMax);
        currentMax = Mathf.Clamp(currentMax, currentMin, _max);

        minProp.floatValue = _min;
        maxProp.floatValue = _max;

        this.currentMinProp.floatValue = currentMin;
        this.currentMaxProp.floatValue = currentMax;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * numberOfLines;
    }
}
