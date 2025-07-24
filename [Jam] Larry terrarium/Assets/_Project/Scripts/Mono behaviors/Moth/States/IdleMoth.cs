using System;
using System.Collections;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class IdleMoth : MothState
{
    [TitleGroup("Bezier movement")]
    [HideLabel]
    [SerializeField]
    private BezierMovementSettings movementSettings;

    [TitleGroup("Looking for light settings")]
    [HideLabel]
    [SerializeField]
    private LookingForLightSettings lookingForLightSettings;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool isGoingUp;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float distanceFromGround;
    
    private Vector3 nextMovementPosition;
    
    private NTask movementRoutine;
    private NTask lookingForLightRoutine;
    
    public override void Enter()
    {
        Moth.Animator.Play("moth");
        
        isGoingUp = Random.value < .5f;
        
        SetTaskForNewPoint(false);

        lookingForLightRoutine = new NTask(LookingForLightRoutine(), new NTask.Settings() { startOnNextFrame = true });

        Moth.OnUpdateHungerAmount += HungerUpdateHandle;
    }

    public override void Exit()
    {
        movementRoutine.Stop();
        lookingForLightRoutine.Stop();

        Moth.OnUpdateHungerAmount -= HungerUpdateHandle;
    }

    public IEnumerator LookingForLightRoutine()
    {
        var foundLights = new Collider2D[5]; 
        while (true)
        {
            if (Moth == null)
                yield break;
            
            var finalPosition = Moth.transform.position + (Vector3)Moth.ForwardDirection * lookingForLightSettings.lookingDistance;
            var amountOfFoundLights =
                Physics2D.OverlapCircle(finalPosition, 1.5f, lookingForLightSettings.contactFilter, foundLights);
            
            if (amountOfFoundLights == 0)
            {
                yield return new WaitForSeconds(lookingForLightSettings.timeBetweenChecks);
                continue;
            }
            
            var validLight = foundLights
                    .Take(amountOfFoundLights)
                    .Select(c => c.GetComponentInParents<ILight>(Extension.Self.Exclude))
                    .FirstOrDefault(l => l.IsOn);

            if (validLight == null)
            {
                yield return new WaitForSeconds(lookingForLightSettings.timeBetweenChecks);
                continue;
            }

            Moth.TargetLight = foundLights[0].GetComponentInParents<ILight>(Extension.Self.Exclude);
            Moth.ChangeState(Moth.GoingToLightState);
            break;
        }
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(nextMovementPosition, .25f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Moth.transform.position, -Moth.transform.up * DistanceToGround());
    }

    private void SetTaskForNewPoint (bool manualFinish)
    {
        if (manualFinish)
            return;

        var settings = movementSettings;
        settings.isGoingUp = isGoingUp;
        
        movementRoutine = new NTask(MovementRoutine());
        movementRoutine.OnFinished += manual =>
        {
            isGoingUp = !isGoingUp;
            
            SetTaskForNewPoint(manual);
        };

        IEnumerator MovementRoutine()
        {
            nextMovementPosition = nextMovementPosition = GetNewTargetPoint();
            AddHeightToTargetPoint();

            settings.targetPoint = nextMovementPosition;
            
            if (Moth.transform.position.x < nextMovementPosition.x)
                Moth.FaceRight();
            else
                Moth.FaceLeft();
            
            yield return BezierMovement(settings);
            
            if (Random.value < .33f)
                yield return new WaitForSeconds(1.5f); 
        }
    }

    private void AddHeightToTargetPoint ()
    {
        distanceFromGround = DistanceToGround();

        var validRange = Moth.HeightValidRange;
        if (distanceFromGround >= validRange.x && distanceFromGround <= validRange.y)
            return;
        
        var validHeight = validRange.GetRandom() - distanceFromGround;
        nextMovementPosition.y += validHeight;
    }

    [Button]
    [DisableInEditorMode]
    private void Stop()
    {
        movementRoutine.Stop();
    }
    
    [Serializable]
    public class LookingForLightSettings
    {
        public float lookingDistance = 2f;
        
        [Range(0.5f, 2f)]
        public float timeBetweenChecks = .5f;

        public ContactFilter2D contactFilter;
    }
}