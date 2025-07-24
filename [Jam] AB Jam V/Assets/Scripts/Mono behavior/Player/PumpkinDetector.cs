using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PumpkinDetector : MonoBehaviour
{
    [Header("Status")]
    public float pumpkinDetectorRange = 1f;
    public List<Pumpkin> nearPumpkins;
    
    [Header("Reference")]
    [SerializeField] private CircleCollider2D pumpkinDetector;

    // [Header("Debug")]
    
    #region Monobehaviour callbacks

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.forward, pumpkinDetectorRange);
        
        Handles.color = Color.yellow;
        foreach (var pumpkin in nearPumpkins)
        {
            Handles.DrawAAPolyLine(Texture2D.whiteTexture, transform.position, pumpkin.transform.position);
        }
    }
#endif

    private void OnValidate()
    {
        pumpkinDetector.radius = pumpkinDetectorRange;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.TryGetComponent<Pumpkin>(out var pumpkin))
            nearPumpkins.Add(pumpkin);
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.TryGetComponent<Pumpkin>(out var pumpkin))
            nearPumpkins.Remove(pumpkin);
    }

    #endregion
}