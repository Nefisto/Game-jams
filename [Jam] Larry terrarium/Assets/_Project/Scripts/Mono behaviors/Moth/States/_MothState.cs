using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public abstract class MothState : State
{
    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(0, 10)]
    [field: SerializeField]
    public virtual Vector2Int HungerLossRate { get; set; } = Vector2Int.one;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public virtual float SpawnSeedSpeedMultiplier { get; set; } = 1f;    
    
    public Moth Moth => machine as Moth;

    public virtual void GiveUpFood()
    {
        if (Moth.TargetFood is not null)
            Moth.TargetFood.RemovePotentialEater(Moth);
        
        Moth.ChangeState(Moth.IdleState);
    }

    public Vector2 GetRandomPositionInsideLightRadius
        => (Vector2)Moth.TargetLight.Transform.position + Random.insideUnitCircle * (Moth.TargetLight.Radius * .75f);

    protected void HungerUpdateHandle()
    {
        if (Moth.IsHungry)
            Moth.ChangeState(Moth.LookingForFoodState);
    }
    
    protected Vector3 GetNewTargetPoint()
    {
        if (machine == null)
            return Vector3.zero;
        
        var mothTransform = machine.transform;
        var distance = Random.Range(0.75f, 1.5f);
        
        var direction = (Vector3)Moth.ForwardDirection;
        direction *= Random.value <= .5f ? 1f : -1f;
        
        return CanMoveToDirection(direction, distance)
            ? mothTransform.position + direction * distance
            : mothTransform.position + -direction * distance;
    }
    
    protected float DistanceToGround()
    {
        if (Moth == null)
            return 0f;
        
        var t = Moth.transform;
        var rayHit = Physics2D.Raycast(t.position, -t.up, Mathf.Infinity, LayerMask.GetMask("Ground"));

        return rayHit.distance;
    }

    protected bool CanMoveToDirection (Vector2 direction, float distance) 
        => !Physics2D.BoxCast(Moth.transform.position, Vector2.one * .5f, 0f, direction, distance, LayerMask.GetMask("Wall"));
}