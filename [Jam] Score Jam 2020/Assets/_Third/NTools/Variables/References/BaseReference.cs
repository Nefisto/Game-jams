using System;

public class BaseReference<T, K>
    where K : BaseVariable<T>
{
    public bool UseConstant = true;
    public T ConstantValue;
    public K Variable;

    public void RegisterOnBeforeChangeValue(Action<T> listener)
        => Variable.BeforeChangeValue += listener;

    public void RegisterOnAfterChangeValue(Action<T> listener)
        => Variable.AfterChangeValue += listener;

    protected BaseReference() { }
    
    public void ChangeFuncThatRunBeforeChangeValue(Func<T, T> func)
        => Variable.ChangeFuncThatRunBeforeChangedValue(func);
    
    public BaseReference(T value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public T Value
    {
        get => UseConstant ? ConstantValue : Variable.Value;
        set
        {
            if (UseConstant)
            {
                ConstantValue = value;
            }
            else // Variable will be responsible to call events
                Variable.Value = value;

        }
    }

    public override string ToString()
        => Value.ToString();
}