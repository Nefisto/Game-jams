using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class IdleState : DogState
{
    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(0.5f, 2.5f)]
    [field: SerializeField]
    public Vector2 IdleDuration { get; set; } = new(1f, 2f);

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float randomizedDuration;

    [TitleGroup("Debug")]
    [ProgressBar(0f, 1f)]
    [ReadOnly]
    [ShowInInspector]
    private float percentageToComplete;

    private IEnumerator idleRoutine;
    
    public override void Enter()
    {
        randomizedDuration = IdleDuration.GetRandom();

        idleRoutine = IdleRoutine();
        Dog.StartCoroutine(idleRoutine);
        
        Dog.Animator.Play("idle");
    }

    public override void Exit()
    {
        Dog.StopCoroutine(idleRoutine);
    }

    private IEnumerator IdleRoutine()
    {
        yield return new WaitForSecondsActioningUntil(randomizedDuration, f => percentageToComplete = f / randomizedDuration);

        if (Dog.IsHungry)
        {
            Dog.ChangeState(Dog.LookingForFoodState);
            yield break;
        }
        
        Dog.ChangeState(Dog.WanderingState);
    }
}