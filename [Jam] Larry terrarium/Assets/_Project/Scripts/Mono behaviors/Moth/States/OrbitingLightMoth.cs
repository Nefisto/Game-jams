using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class OrbitingLightMoth : MothState
{
    [TitleGroup("Settings")]
    [HideLabel]
    [SerializeField]
    private BezierMovementSettings movementSettings;

    private NTask orbitingRoutine;
    
    public override void Enter()
    {
        orbitingRoutine = new NTask(OrbitingRoutine());

        Moth.OnUpdateHungerAmount += HungerUpdateHandle;
    }

    public override void Exit()
    {
        orbitingRoutine?.Stop();
        
        Moth.OnUpdateHungerAmount -= HungerUpdateHandle;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(movementSettings.targetPoint, .2f);
    }

    private IEnumerator OrbitingRoutine()
    {
        while (true)
        {
            movementSettings.targetPoint = GetRandomPositionInsideLightRadius;
            yield return BezierMovement(movementSettings, MovementUpdateHandle);

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    private void MovementUpdateHandle()
    {
        var isLightAtMothRight = (Moth.TargetLight.Transform.position - Moth.transform.position).x > 0;
        
        if (isLightAtMothRight)
            Moth.FaceRight();
        else
            Moth.FaceLeft();
    }
}