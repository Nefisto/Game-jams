using UnityEngine;

public partial class Extensions
{
    public static Vector2Int ToVector2Int(this Vector2 vector2)
        => new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));

    public static float ToDegreeAngle(this Vector2 vector)
        => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

    public static float TimeToReachDestination (this Vector2 initialPosition, Vector2 targetPosition, float speed)
    {
        var distance = Vector2.Distance(initialPosition, targetPosition);

        return distance / speed;
    }
}