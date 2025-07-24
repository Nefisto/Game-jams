using UnityEngine;

public static partial class Extensions
{
    // Custom editor n propertyDrawer
    /// <summary>
    /// Center a checkbox inside a given rect
    /// </sumary>
    public static Rect CenterCheckBox(this Rect rect)
    {
        var centeredPosition = rect.center - new Vector2(7.5f, 9f);

        return new Rect(centeredPosition, rect.size);
    }

    /// <summary>
    /// Give n percent of given rect
    /// </sumary>
    public static Vector2 ResizeX(this Rect rect, float percentX = 1f)
    {
        Vector2 newSize = rect.size;

        newSize.x *= percentX;

        return newSize;
    }
}