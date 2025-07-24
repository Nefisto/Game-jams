public class RuntimeItem : LazyBehavior
{
    public RuntimeSet runtimeSet;

    private void OnEnable()
        => runtimeSet.Add(this);

    private void OnDisable()
        => runtimeSet.Remove(this);
}
