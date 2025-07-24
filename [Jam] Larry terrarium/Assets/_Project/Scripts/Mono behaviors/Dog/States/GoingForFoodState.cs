using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GoingForFoodState : DogState
{
    [FormerlySerializedAs("minDistanceInYToGiveUpFood")]
    [TitleGroup("Settings")]
    [SerializeField]
    private float distanceInYToGiveUpFood = 1.5f;
    
    [TitleGroup("Settings")]
    [SerializeField]
    private float movementSpeed = .5f;
    
    [TitleGroup("Settings")]
    [SerializeField]
    private float distanceToEat = .5f;

    private IEnumerator reachingNearFoodRoutine;
    
    public override void Enter()
    {
        Dog.Animator.Play("stealth");
        
        reachingNearFoodRoutine = MovementRoutine();

        Dog.StartCoroutine(reachingNearFoodRoutine);

        Dog.TargetFood.OnDestroyed += DestroyedFoodHandle;
        
    }

    public override void Exit()
    {
        Dog.StopCoroutine(reachingNearFoodRoutine);
        
        Dog.TargetFood.OnDestroyed -= DestroyedFoodHandle;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Dog.TargetFood.Transform.position, .25f);
    }

    public void DestroyedFoodHandle() => Dog.ChangeState(Dog.LookingForFoodState);

    private IEnumerator MovementRoutine()
    {
        while (Mathf.Abs(MouthPoint.x - Dog.TargetFood.Transform.position.x) > distanceToEat)
        {
            Dog.transform.Translate(new Vector2(movementSpeed, 0f) * Time.deltaTime);
            yield return null;
            
            var distanceInYFromFood = Mathf.Abs(Dog.TargetFood.Transform.position.y - MouthPoint.y);

            if (distanceInYFromFood <= distanceInYToGiveUpFood)
                continue;
            
            Dog.ChangeState(Dog.ConfuseState);
            yield break;
        }
            
        Dog.ChangeState(Dog.EatingState);
    }
}