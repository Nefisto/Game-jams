using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ConfuseDogState : DogState
{
    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(1f, 3f)]
    [field: SerializeField]
    public Vector2 ConfuseDuration { get; private set; } = new(1.5f, 2.5f);

    [TitleGroup("Debug")]
    [ShowInInspector]
    private float randomizedDuration;

    public override void Enter()
    {
        Dog.Animator.Play("confused");
        randomizedDuration = ConfuseDuration.GetRandom();

        Dog.StartCoroutine(ConfuseRoutine());
    }

    public override void Exit()
    {
        
    }

    private IEnumerator ConfuseRoutine()
    {
        yield return new WaitForSeconds(randomizedDuration);
        
        Dog.ChangeState(Dog.IdleState);
    }
}