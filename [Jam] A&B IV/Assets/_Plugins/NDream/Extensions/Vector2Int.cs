using UnityEngine;

public partial class Extensions
{
    public static float ToDegreeAngle(this Vector2Int vector)
        => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
}