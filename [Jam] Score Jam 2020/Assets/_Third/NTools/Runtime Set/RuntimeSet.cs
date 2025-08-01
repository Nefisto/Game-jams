using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "RuntimeSet", menuName= "Framework/RuntimeSet")]
public class RuntimeSet : ScriptableObject
{
    public List<RuntimeItem> Items = new List<RuntimeItem>();

    public int Count
    {
        get => Items.Count;
    }
    
    public void Add(RuntimeItem thing)
    {
        if (!Items.Contains(thing))
            Items.Add(thing);
    }

    public void Remove(RuntimeItem thing)
    {
        if (Items.Contains(thing))
            Items.Remove(thing);
    }

    public bool TryGetFromItens<T>(ref List<T> list)
    {
        foreach (var item in Items)
        {
            if (!item.TryGetComponent<T>(out var currentT))
                return false;
            
            list.Add(currentT);
        }

        return true;
    }
}