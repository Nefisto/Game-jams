using UnityEngine;

public partial class Extensions
{
    public static Vector2Int ToVector2Int(this Vector3 vector3)
        => new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
}