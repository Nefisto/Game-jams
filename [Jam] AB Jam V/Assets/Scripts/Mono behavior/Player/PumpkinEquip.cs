using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PumpkinEquip : MonoBehaviour
{
    // Previous, New
    public event Action<Pumpkin, Pumpkin> OnChangeEquippedPumpkin;

    [Header("Status")]
    [ReadOnly] public Pumpkin equippedPumpkin;
    public Transform equippedPosition;
    
    [Header("References")]
    [SerializeField] private PumpkinDetector pumpkinDetector;

    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer faceRenderer;
    
    [Header("Debug")]
    [SerializeField, ReadOnly] private bool canEquipPumpkin = true;
    [SerializeField] private Pumpkin lastEquippedPumpkin;
    
    #region API

    public void EnableHead(Pumpkin groundPumpkin)
    {
        if (lastEquippedPumpkin != null)
        {
            lastEquippedPumpkin.OnDie -= DisableHead;
        }
        
        lastEquippedPumpkin = groundPumpkin;
        groundPumpkin.DisableGraphics();

        headRenderer.enabled = true;
        faceRenderer.enabled = true;
        
        groundPumpkin.OnDie += DisableHead;
    }

    public void DisableHead()
    {
        headRenderer.enabled = false;
        faceRenderer.enabled = false;
    }
    
    public void WearPumpkin()
    {
        if (!canEquipPumpkin)
            return;
        
        if (pumpkinDetector.nearPumpkins.Count == 0)
            return;

        var validPumpkins = pumpkinDetector
            .nearPumpkins
            .Where(pumpkin => !pumpkin.leftSpot.crow && !pumpkin.rightSpot.crow)
            .ToList();
        
        if (validPumpkins.Count == 0)
        {
            GameEvents.OnError?.Invoke(); // Can't equip head
            return;
        }

        var lowerIndex = 0;
        var lowerDistance = Vector2.SqrMagnitude(validPumpkins[0].transform.position - transform.position);

        for (var i = 1; i < validPumpkins.Count; i++)
        {
            var distance = Vector2.SqrMagnitude(validPumpkins[i].transform.position - transform.position);

            if (distance < lowerDistance)
            {
                lowerIndex = i;
                lowerDistance = distance;
            }
        }
        
        EquipPumpkin(validPumpkins[lowerIndex]);
    }

    public void ReleaseCurrentPumpkin()
    {
        
    }
    
    #endregion

    #region Private Methods

    private void EquipPumpkin (Pumpkin newPumpkin)
    {
        OnChangeEquippedPumpkin?.Invoke(equippedPumpkin, newPumpkin);

        if (equippedPumpkin != null)
            equippedPumpkin.ChangeState(PumpkinState.OnEarth);
        
        equippedPumpkin = newPumpkin;
        StartCoroutine(GoToHeadPosition());

        equippedPumpkin.ChangeState(PumpkinState.OnHead);

        IEnumerator GoToHeadPosition()
        {
            canEquipPumpkin = false;
            while (Vector2.SqrMagnitude(equippedPumpkin.transform.position - equippedPosition.transform.position) > .1f)
            {
                var newPos = Vector2.Lerp(equippedPumpkin.transform.position, equippedPosition.transform.position, .1f);

                equippedPumpkin.transform.position = newPos;
                
                yield return null;
            }

            equippedPumpkin.transform.position = equippedPosition.transform.position;
            equippedPumpkin.transform.parent = equippedPosition.transform;
            
            canEquipPumpkin = true;
            
            EnableHead(newPumpkin);
        }
    }

    #endregion
}