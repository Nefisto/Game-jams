
[System.Serializable]
public class FloatReference : BaseReference<float, FloatVariable>
{
    public static implicit operator float(FloatReference reference)
        => reference.Value;
}