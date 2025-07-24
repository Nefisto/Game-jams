using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WanderingState : DogState
{
    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(1f, 5f)]
    [field: SerializeField]
    public Vector2 WanderingDuration { get; private set; } = new(2f, 4f); 
    
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public float HorizontalSpeed { get; private set; }

    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(1f, 10f)]
    [field: SerializeField]
    public Vector2 MoveDistanceRange { get; set; } = new(1f, 5f);
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Vector2 targetPosition;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float randomizedDirection;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float randomizedDistance;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float randomizedDuration;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Vector2 lookingPoint;

    private NTask movementRoutine;
    private IEnumerator visionRoutine;

    public override void Enter()
    {
        randomizedDirection = Random.value < .5f ? 1f : -1f;
        randomizedDuration = WanderingDuration.GetRandom();
        
        LookAtCorrectDirection();
        
        movementRoutine = new NTask(MovementRoutine(), new NTask.Settings() { startOnNextFrame = true });
        visionRoutine = VisionCheckRoutine();
        
        Dog.StartCoroutine(visionRoutine);
        
        Dog.Animator.Play("walk");
    }

    public override void Exit()
    {
        movementRoutine.Stop();
        Dog.StopCoroutine(visionRoutine);
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(lookingPoint, .25f);
    }

    private void LookAtCorrectDirection()
    {
        var newAngle = randomizedDirection < 0 ? 0f : 180f;
        Dog.transform.rotation = Quaternion.Euler(0f, newAngle, 0f);
    }

    private IEnumerator MovementRoutine()
    {
        var timer = 0f;
        while (timer <= randomizedDuration)
        {
            Dog.transform.Translate(new Vector2(HorizontalSpeed, 0f) * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
        
        while (!Dog.OnGround)
            yield return null;
        
        Dog.ChangeState(Dog.IdleState);
    }
    
    private IEnumerator VisionCheckRoutine()
    {
        while (true)
        {
            lookingPoint = MouthPoint + (Vector2)Dog.transform.right;

            if (TryGetObjectThatBlockView(.15f, out var hit))
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
}