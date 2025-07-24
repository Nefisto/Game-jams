using UnityEngine;

[System.Serializable]
public class Vector2IntReference : BaseReference<Vector2Int, Vector2IntVariable>
{
    public int x
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

    public int y
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

    public static implicit operator Vector2Int(Vector2IntReference reference)
        => reference.Value;
}