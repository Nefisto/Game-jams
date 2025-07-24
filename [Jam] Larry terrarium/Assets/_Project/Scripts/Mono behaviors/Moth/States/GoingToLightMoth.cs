
using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GoingToLightMoth : MothState
{
    [TitleGroup("Settings")]
    [SerializeField]
    private float idleTimeWhenLightTurnOffBeforeIReach = 2f;
    
    [TitleGroup("Bezier movement")]
    [HideLabel]
    [SerializeField]
    private BezierMovementSettings movementSettings;
    
    private NTask goingToLightRoutine;
    
    public override void Enter()
    {
        movementSettings.targetPoint = GetRandomPositionInsideLightRadius; 
        goingToLightRoutine = new NTask(BezierMovement(movementSettings));
        goingToLightRoutine.OnFinished += manualStop =>
        {
            if (manualStop)
                return;

            Moth.ChangeState(Moth.OrbitingLightState);
        };

        Moth.TargetLight.OnTurnedOff += OffHandle;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Moth.TargetLight.Transform.position, .25f);
    }

    private void OffHandle()
    {
        if (Moth == null)
            return;
        
        Moth.TargetLight.OnTurnedOff -= OffHandle;
        goingToLightRoutine.Pause();
        
        Moth.StartCoroutine(Routine());
        
        IEnumerator Routine()
        {
            // TODO: Flip image to left right sometimes to give the impression of confusion
            yield return new WaitForSeconds(idleTimeWhenLightTurnOffBeforeIReach);
            
            Moth.ChangeState(Moth.IdleState);
        }
    }

    public override void Exit()
    {
        goingToLightRoutine.Stop();
    }
}