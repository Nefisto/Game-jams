using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class EatingStateDog : DogState
{
    [TitleGroup("Settings")]
    [SerializeField]
    private float eatingDuration = 2f;
    
    private IEnumerator eatingRoutine;
    
    public override void Enter()
    {
        eatingRoutine = EatingRoutine();
        Dog.StartCoroutine(eatingRoutine);
    }

    public override void Exit()
    {
        Dog.StopCoroutine(eatingRoutine);
    }

    private IEnumerator EatingRoutine()
    {
        Dog.Animator.Play("eat");
        Dog.DogEatSfx.Play();
        
        Dog.TargetFood.RemovePotentialEater(Dog);
        Dog.TargetFood.MakePotentialEaterToGiveUp();
        Dog.TargetFood.Kill();
        Dog.TargetFood.DisablePhysics();
        Dog.TargetFood.DisableDetectors();
        
        var moveTween = Dog.TargetFood.Transform.DOMove(MouthPoint, eatingDuration);
        yield return moveTween.WaitForCompletion();

        Dog.TargetFood.BeConsumed();
        Dog.CurrentHungry += Dog.TargetFood.EnergyProvided;
        yield return null;
        Object.Destroy(Dog.TargetFood.Transform.gameObject);
        Dog.TargetFood = null;
        Dog.ChangeState(Dog.IdleState);
    }
}