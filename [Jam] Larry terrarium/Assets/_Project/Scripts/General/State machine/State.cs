using System;
using System.Collections;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public abstract class State
{
    public string StateName { get; private set; }
    protected StateMachine machine;

    public virtual void Setup (Settings settings)
    {
        StateName = settings.name;
        machine = settings.machine;
    }
    
    public abstract void Enter();
    public abstract void Exit();
    public virtual void OnDrawGizmosSelected() { }

    protected IEnumerator BezierMovement (BezierMovementSettings settings, Action movementUpdateCallback = null)
    {
        var actor = machine.transform;
        
        
        var p1 = actor.position;
        var p2 = settings.targetPoint;
                
        var p1ControlDirection = (settings.isGoingUp ? actor.up : -actor.up) * settings.bezierControlMultiplier;
        var p2ControlDirection = (settings.isGoingUp ? actor.up : -actor.up) * settings.bezierControlMultiplier;
                
        var distance = Vector3.Distance(p1, p2);
                
        var percentage = 0f;
        var travelledAmount = 0f;
        while (travelledAmount < distance)
        {
            if (actor == null)
                yield break;
            
            actor.position = DOCurve.CubicBezier.GetPointOnSegment(p1, p1 + p1ControlDirection, p2, p2 + p2ControlDirection, percentage);

            travelledAmount += settings.speed * Time.deltaTime;
            percentage = travelledAmount / distance;
                         
            movementUpdateCallback?.Invoke();
            yield return null;
        }

        actor.position = p2;
    }

    [Serializable]
    public class BezierMovementSettings
    {
        public float speed = 1f;
        public float bezierControlMultiplier = 1f;
        
        [HideInInspector]
        public Vector3 targetPoint;
        [HideInInspector]
        public bool isGoingUp;
    }
    
    public class Settings
    {
        public string name;
        public StateMachine machine;
    }
}