[System.Serializable]
public class IntReference : BaseReference<int, IntVariable>
{
    public static implicit operator int(IntReference reference)
        => reference.Value;

    public static IntReference operator --(IntReference reference)
    {
        reference.Value--;

        return reference;
    }

    public static IntReference operator ++(IntReference reference)
    {
        reference.Value++;

        return reference;
    }
}