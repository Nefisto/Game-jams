
using System;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class GoingToFoodMoth : MothState
{
    [TitleGroup("Bezier movement")]
    [HideLabel]
    [SerializeField]
    private BezierMovementSettings movementSettings;

    private NTask movementRoutine;
    
    public override void Enter()
    {
        movementSettings.targetPoint = Moth.TargetFood.Transform.position;
        
        movementRoutine = new NTask(BezierMovement(movementSettings));
        movementRoutine.OnFinished += manualStop =>
        {
            if (manualStop)
                return;
            
            Moth.ChangeState(Moth.EatingState);
        };

        Moth.TargetFood.RegisterPotentialEater(Moth);
        Moth.TargetFood.OnBecomeInedible += GiveUpFood;
        Moth.TargetFood.OnBeConsumed += GiveUpFood;
    }
    
    public override void Exit()
    {
        movementRoutine.Stop();
        
        Moth.TargetFood.OnBecomeInedible -= GiveUpFood;
        Moth.TargetFood.OnBeConsumed -= GiveUpFood;
    }

    public override void GiveUpFood()
    {
        Moth.TargetFood.RemovePotentialEater(Moth);

        Moth.ChangeState(Moth.ConfuseState);
    }

    private void FoodDieHandle() => Moth.ChangeState(Moth.ConfuseState);

    private void BecomingInedibleHandler() => Moth.ChangeState(Moth.ConfuseState);
}