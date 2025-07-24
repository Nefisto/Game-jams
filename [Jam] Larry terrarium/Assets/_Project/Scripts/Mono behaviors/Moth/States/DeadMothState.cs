using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DeadMothState : MothState
{
    [TitleGroup("References")]
    [SerializeField]
    private Transform feetCollider;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private bool OnGround { get; set; }
    
    [TitleGroup("Debug")]
    [ShowInInspector]
    private Vector2 Velocity { get; set; }

    private IEnumerator movementRoutine;
    private Collider2D[] groundContactCollider = new Collider2D[5];
    
    public override void Enter()
    {
        Moth.Animator.Play("death");
        Moth.CanSpawnSeed = false;
        Moth.CanIncreaseHungry = false;

        movementRoutine = MovementRoutine();
        Moth.StartCoroutine(movementRoutine);
    }

    public override void Exit()
    {
        Moth.StopCoroutine(movementRoutine);
    }

    private IEnumerator MovementRoutine()
    {
        while (true)
        {
            if (Moth.CanApplyPhysics)
                Velocity += new Vector2(0f, Physics2D.gravity.y * .5f * Time.deltaTime);

            ValidateGround();
        
            Moth.transform.Translate(Velocity * Time.deltaTime);
            yield return null;
        }
    }
    
    private void ValidateGround()
    {
        if (Velocity.y < 0f
            && Physics2D.OverlapBoxNonAlloc(feetCollider.position, feetCollider.localScale, 0f,
                groundContactCollider, LayerMask.GetMask("Ground"))
            > 0)
        {
            Velocity = new Vector2(0f, 0f);

            var surface = Physics2D.ClosestPoint(Moth.transform.position, groundContactCollider[0]);
            Moth.transform.position = new Vector2(Moth.transform.position.x, surface.y);

            if (OnGround)
                return;
            
            OnGround = true;
        }
    }
}