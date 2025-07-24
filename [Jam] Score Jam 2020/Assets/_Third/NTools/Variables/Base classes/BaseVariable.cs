using System;
using UnityEngine;

public abstract class BaseVariable<T> : ScriptableObject
{
#pragma warning disable 0414

    [SerializeField]
    [Multiline]
    private string developerDescription = "";

#pragma warning restore 0414

    [Header("Will have an default value?")]
    public bool haveDefaultValue = false;

    [Header("Default/Current value")]
    [SerializeField]
    private T value;
    private T oldValue;
    
    [Header("Debug")]
    [SerializeField]
    private T runTimeValue;
    
    public event Action<T> BeforeChangeValue;
    public event Action<T> AfterChangeValue;

    private event Func<T, T> runBeforeApplyChangeValue;

    public void ChangeFuncThatRunBeforeChangedValue(Func<T, T> func)
    {
        runBeforeApplyChangeValue = func;
    }
        
    public T Value
    {
        get => haveDefaultValue ? runTimeValue : value;
        set
        {
            // Debug.Log("IN VARIABLE");
            if (haveDefaultValue)
            {
                OnBeforeChangeValue(runTimeValue);
                
                runTimeValue = value;
            }
            else
            {
                OnBeforeChangeValue(this.value);
                
                this.value = value;
            }
            
            OnAfterChangeValue(value);
        }
    }

    // To run events when change values from inspector \\o//
    private void OnValidate()
    {
        if (oldValue.Equals(Value))
            return;

        T _oldValue = runBeforeApplyChangeValue != null 
            ? runBeforeApplyChangeValue.Invoke(oldValue) 
            : oldValue;

        T _newValue = runBeforeApplyChangeValue != null
            ? runBeforeApplyChangeValue.Invoke(Value)
            : Value;
        
        OnBeforeChangeValue(_oldValue);
        OnAfterChangeValue(_newValue);

        oldValue = Value;
    }

    public void Reset()
        => Value = default(T);

    private void OnEnable()
    {
        if (haveDefaultValue)
            runTimeValue = value;
    }

    private void OnDisable()
    {
        if (haveDefaultValue)
            runTimeValue = value;
    }

    public override string ToString()
        => Value.ToString();

    public void OnBeforeChangeValue(T obj)
        => BeforeChangeValue?.Invoke(obj);

    public void OnAfterChangeValue(T obj)
        => AfterChangeValue?.Invoke(obj);
}