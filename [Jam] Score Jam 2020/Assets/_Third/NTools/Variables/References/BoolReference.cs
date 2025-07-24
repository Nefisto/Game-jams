
[System.Serializable]
public class BoolReference : BaseReference<bool, BoolVariable>
{
    public void Toggle()
    {
        if (ConstantValue)
            Value = !Value;
        else
            Variable.Toggle();
    }

    public static implicit operator bool(BoolReference reference)
        => reference.Value;
}