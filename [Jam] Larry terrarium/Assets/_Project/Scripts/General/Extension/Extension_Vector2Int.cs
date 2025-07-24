using UnityEngine;

public static partial class Extension
{
    public static int GetRandom (this Vector2Int vector2)
        => Random.Range(vector2.x, vector2.y);
}