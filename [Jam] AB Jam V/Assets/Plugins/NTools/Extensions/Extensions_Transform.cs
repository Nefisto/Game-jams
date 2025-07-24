using UnityEngine;

public partial class Extensions
{
    public static void RotateYawTo (this Transform transform, float newYaw)
        => transform.eulerAngles = new Vector3(0f, newYaw, 0f);

    public static void RotatePitch (this Transform transform, float newPitch)
        => transform.eulerAngles = new Vector3(0f, 0f, newPitch);

    public static void RotateRoll (this Transform transform, float newRoll)
        => transform.eulerAngles = new Vector3(newRoll, 0f, 0f);
}