using System;
using UnityEngine;

public class ColliderPropagation : MonoBehaviour
{
    public event Action<Collider2D> OnEnterContact;

    [SerializeField]
    private LayerMask layerMask; 
    
    private void OnTriggerEnter2D (Collider2D other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) == 0)
            return;
        
        OnEnterContact?.Invoke(other);
    }
}