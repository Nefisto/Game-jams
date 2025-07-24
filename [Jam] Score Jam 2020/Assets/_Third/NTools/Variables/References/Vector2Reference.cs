using UnityEngine;

[System.Serializable]
public class Vector2Reference : BaseReference<Vector2, Vector2Variable>
{
    public float x
    {
        get => Value.x;
        set
        {
            if (UseConstant)
                ConstantValue.x = value;
            else
                Variable.x = value;
        }
    }

    public float y
    {
        get => Value.y;
        set
        {
            if (UseConstant)
                ConstantValue.y = value;
            else
                Variable.y = value;
        }
    }

    public static implicit operator Vector2(Vector2Reference reference)
        => reference.Value;
}