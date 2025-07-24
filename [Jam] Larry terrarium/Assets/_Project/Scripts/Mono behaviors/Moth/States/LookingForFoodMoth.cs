using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class LookingForFoodMoth : MothState
{
    [TitleGroup("Ground check")]
    [HideLabel]
    [SerializeField]
    private CheckForGroundSettings checkForGroundSettings;

    [TitleGroup("Plants check")]
    [HideLabel]
    [SerializeField]
    private CheckForPlantSettings checkForPlantSettings;
    
    [TitleGroup("Bezier movement")]
    [HideLabel]
    [SerializeField]
    private BezierMovementSettings movementSettings;

    private Vector3 foundGroundPosition;

    private List<Transform> foundPlants = new();

    private Ray groundCheckRay;
    private bool hasFoundGround;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool isGoingUp;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float distanceFromGround;

    private NTask lookingForPlantRoutine;
    private NTask movementRoutine;

    private Vector3 nextMovementPosition;

    public override void Enter()
    {
        lookingForPlantRoutine = new NTask(LookingForPlantRoutine(), new NTask.Settings() { startOnNextFrame = true });
        SetTaskForNewPoint(false);
    }

    public override void Exit()
    {
        lookingForPlantRoutine.Stop();
        movementRoutine.Stop();
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(groundCheckRay.origin, groundCheckRay.direction * 10f);

        if (!hasFoundGround)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(foundGroundPosition, checkForPlantSettings.plantDetectionBoxSize);

        foreach (var foundPlant in foundPlants)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(foundPlant.position, .25f);
        }
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

    private IEnumerator LookingForPlantRoutine()
    {
        var cachedGrounds = new List<RaycastHit2D>();
        var cachedPlants = new Collider2D[20];
        while (true)
        {
            foundPlants.Clear();
            
            var directionVector = Moth.ForwardDirection;
            var mothTransform = Moth.transform;
            groundCheckRay = new Ray(mothTransform.position, -(Vector2)mothTransform.up + directionVector);

            var groundPoint = Physics2D.Raycast(groundCheckRay.origin, groundCheckRay.direction,
                checkForGroundSettings.contactFilter2D, cachedGrounds, 10f);

            hasFoundGround = groundPoint != 0;
            if (groundPoint == 0)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            var nearestGround = cachedGrounds[0].transform;
            foundGroundPosition =
                nearestGround.position
                + new Vector3(0f, .5f, 0f); // To go from center to the upper part of ground sprite
            var amountOfPlantsFound = Physics2D.OverlapBox(foundGroundPosition,
                checkForPlantSettings.plantDetectionBoxSize, 0f, checkForPlantSettings.contactFilter2D, cachedPlants);

            if (amountOfPlantsFound == 0)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            foundPlants = cachedPlants
                .Take(amountOfPlantsFound)
                .Select(p => p.transform)
                .ToList();

            var validPlants = foundPlants
                .Select(p => p.GetComponentInParents<IPlant>(Extension.Self.Exclude))
                .Where(p => p.IsEdible)
                .ToList();

            if (!validPlants.Any())
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            Moth.TargetFood = validPlants.GetRandom();
            Moth.ChangeState(Moth.GoingToFoodState);
            break;
        }
    }

    [Serializable]
    public class CheckForGroundSettings
    {
        public ContactFilter2D contactFilter2D;
    }

    [Serializable]
    public class CheckForPlantSettings
    {
        public ContactFilter2D contactFilter2D;
        public Vector2 plantDetectionBoxSize = new(3f, .25f);
    }
}