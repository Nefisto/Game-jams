using UnityEngine;
using System;

public class PooledObject : MonoBehaviour
{
    public Pool originPool;

    public Action callbacksBeforeDespaw;

    public void Despawn()
    {
        if (callbacksBeforeDespaw != null)
            callbacksBeforeDespaw.Invoke();
            
        originPool.Despawn(gameObject);
    }

    public void Despawn(float timer)
    {
        Invoke(nameof(Despawn), timer);
    }
}
