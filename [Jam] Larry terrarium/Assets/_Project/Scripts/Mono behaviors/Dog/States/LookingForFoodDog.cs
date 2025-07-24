using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class LookingForFoodDog : DogState
{
    [TitleGroup("Settings")]
    [SerializeField]
    private float movementSpeed = 3;

    [TitleGroup("Settings")]
    [MinMaxSlider(1f, 4f)]
    [SerializeField]
    private Vector2 movementDuration;
    
    [TitleGroup("Settings")]
    [SerializeField]
    private float visionRange = 4f;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float randomizedDirection;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float randomizedDuration;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Vector2 lookingPoint;
    
    private Collider2D[] cachedFoods = new Collider2D[20];

    private NTask lookingForFoodRoutine;
    private IEnumerator movementRoutine;
    private NTask visionRoutine;
    
    private List<IFood> possibleFoods = new();

    public override void Enter()
    {
        possibleFoods.Clear();
        lookingForFoodRoutine = new NTask(LookingForFoodRoutine(), new NTask.Settings() { startOnNextFrame = true });
        visionRoutine = new NTask(VisionCheckRoutine(), new NTask.Settings() { startOnNextFrame = true });
        movementRoutine = MovementRoutine();
        
        Dog.StartCoroutine(movementRoutine);
        
        Dog.Animator.Play("walk");
        Dog.OnTouchGround += visionRoutine.Resume;
    }

    public override void Exit()
    {
        Dog.StopCoroutine(movementRoutine);
        
        visionRoutine.Stop();
        lookingForFoodRoutine.Stop();
        
        Dog.OnTouchGround -= visionRoutine.Resume;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GetFoodDetectionBoxCenter(), GetDetectionBoxSize());

        var first = true;
        foreach (var possibleFood in possibleFoods)
        {
            if (!CanSee(possibleFood) || !possibleFood.IsEdible)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawWireSphere(possibleFood.Transform.position, .25f);
                continue;
            }
            
            Gizmos.color = possibleFood.IsMeat ? Color.red : Color.green;
            if (first)
            {
                Gizmos.color = Color.yellow;
                first = false;
            }

            Gizmos.DrawWireSphere(possibleFood.Transform.position, .25f);
        }
    }

    private IEnumerator LookingForFoodRoutine()
    {
        while (true)
        {
            possibleFoods.Clear();
            var foodAmount = Physics2D.OverlapBoxNonAlloc(GetFoodDetectionBoxCenter(), GetDetectionBoxSize(),
                0f, cachedFoods, LayerMask.GetMask("Food"));

            if (foodAmount == 0)
            {
                yield return new WaitForSeconds(0.2f);
                continue;
            }

            possibleFoods = cachedFoods
                .Take(foodAmount)
                .Select(c => c.GetComponentInParents<IFood>(Extension.Self.Exclude))
                .OrderBy(f => f.IsMeat ? 0 : 1)
                .ToList();

            var selectedFood = possibleFoods
                .FirstOrDefault(f => f.IsEdible && CanSee(f));

            if (selectedFood == null)
            {
                yield return new WaitForSeconds(0.2f);
                continue;
            }
            
            Dog.SetFood(selectedFood);
            
            selectedFood.RegisterPotentialEater(Dog);
            selectedFood.OnBeConsumed += GiveUpPlantHandle;
            
            while (!Dog.OnGround)
                yield return null;
            
            Dog.ChangeState(Dog.GoingForFoodState);
            break;
        }
    }
    
    private IEnumerator MovementRoutine()
    {
        while (true)
        {
            randomizedDirection = Random.value < .5f ? 1f : -1f;
            randomizedDuration = movementDuration.GetRandom();
            
            LookAtCorrectDirection();
            
            var timer = 0f;
            while (timer <= randomizedDuration)
            {
                Dog.transform.Translate(new Vector2(movementSpeed, 0f) * Time.deltaTime);

                timer += Time.deltaTime;
                yield return null;
            }

            while (!Dog.OnGround)
                yield return null;
        }
    }
    
    private IEnumerator VisionCheckRoutine()
    {
        while (true)
        {
            lookingPoint = MouthPoint + (Vector2)Dog.transform.right;

            if (TryGetObjectThatBlockView(.25f, out var hit))
            {
                if ((LayerMask.GetMask("Wall") & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    randomizedDirection *= -1;
                    LookAtCorrectDirection();
                }
                else
                {
                    Dog.RequestJump();
                }
            }

            yield return null;
            while (!Dog.OnGround)
                yield return null;
        }
    }
    
    private void LookAtCorrectDirection()
    {
        var newAngle = randomizedDirection < 0 ? 0f : 180f;
        Dog.transform.rotation = Quaternion.Euler(0f, newAngle, 0f);
    }
    
    private bool CanSee(IFood i)
    {
        var hit = Physics2D.Raycast(MouthPoint, i.Transform.position,
            Mathf.Abs(MouthPoint.x - i.Transform.position.x), LayerMask.GetMask("Ground") | LayerMask.GetMask("Wall"));

        return hit.collider is null;
    }

    private Vector2 GetDetectionBoxSize() => new(visionRange, 1.5f);

    private Vector2 GetFoodDetectionBoxCenter() => MouthPoint + (Vector2)Dog.transform.right * (visionRange * .5f);

    private float ValidViewRange()
        => !TryGetObjectThatBlockView(visionRange, out var hit) ? visionRange : Vector2.Distance(MouthPoint, hit.point);
}