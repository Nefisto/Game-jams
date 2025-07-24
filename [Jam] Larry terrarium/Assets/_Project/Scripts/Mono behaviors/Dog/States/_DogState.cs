using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class DogState : State
{
    protected Dog Dog => machine as Dog;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public bool ShouldConsumeHungry { get; set; } = true;

    protected Vector2 MouthPoint
        => (Vector2)machine.transform.position + (Vector2)machine.transform.right * .5f + Vector2.up * .5f;
    
    protected bool TryGetObjectThatBlockView(float range, out RaycastHit2D hit)
    {
        var wallHit = Physics2D.Raycast(MouthPoint, Dog.transform.right, range,
            LayerMask.GetMask("Wall"));

        if (wallHit.collider is not null)
        {
            hit = wallHit;
            return true;
        }
        
        var groundHit = Physics2D.Raycast(MouthPoint, Dog.transform.right, range, 
            LayerMask.GetMask("Ground"));

        hit = groundHit;
        return hit.collider != null;
    }
    
    protected void GiveUpPlantHandle()
    {
        Dog.StartCoroutine(Routine());
        IEnumerator Routine()
        {
            yield return null;
            Dog.ChangeState(Dog.LookingForFoodState);
        }
    }
}