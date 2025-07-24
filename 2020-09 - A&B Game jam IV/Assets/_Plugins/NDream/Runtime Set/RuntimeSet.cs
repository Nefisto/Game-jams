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
}