
using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ConfusedState : MothState
{
    [TitleGroup("Settings")]
    [MinMaxSlider(1f, 3f)]
    [SerializeField]
    private Vector2 confuseDuration = new(1.5f, 2.5f);

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float randomizedDuration;
    
    public override void Enter()
    {
        randomizedDuration = confuseDuration.GetRandom();

        Moth.StartCoroutine(ConfuseRoutine());
    }

    public override void Exit()
    {
        
    }

    private IEnumerator ConfuseRoutine()
    {
        Moth.Animator.Play("confused");
        
        yield return new WaitForSeconds(randomizedDuration);
        
        Moth.ChangeState(Moth.IdleState);
    }
}