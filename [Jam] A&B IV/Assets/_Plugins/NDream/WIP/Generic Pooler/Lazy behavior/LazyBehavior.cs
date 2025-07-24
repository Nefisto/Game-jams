using System;
using UnityEngine;

public abstract partial class LazyBehavior : MonoBehaviour
{
    // Framework components
    [HideInInspector, NonSerialized]
    private PooledObject _pooledObject;
    public PooledObject pooledObject { get => _pooledObject ? _pooledObject : (_pooledObject = GetComponent<PooledObject>()); } 
}
