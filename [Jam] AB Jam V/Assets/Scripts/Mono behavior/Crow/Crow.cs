using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crow : LazyBehavior
{
    [Header("References")]
    public CrowLife crowLife;
    
    [Header("Debug")]
    [ReadOnly] public Pumpkin targetPumpkin;

    public CrowState GetCrowState => crowLife.crowState;
    
    #region Monobehaviour callbacks

    private void OnEnable()
    {
        CrowManager.CrowsInGame.Add(this);
    }

    private void OnDisable()
    {
        CrowManager.CrowsInGame.Remove(this);
    }

    #endregion
    
    #region API

    public void ChooseAPumpkin()
    {
        targetPumpkin = PumpkinManager
            .AlivePumpkins
            .Where(p => p.GetState == PumpkinState.OnEarth && (p.leftSpot.crow == null || p.rightSpot.crow == null))
            .GetRandom();

        if (targetPumpkin == null)
        {
            Destroy(gameObject);
            return;
        }
        
        targetPumpkin.OnDie += RunAway;

        StartCoroutine(GoToPumpkin());
    }

    public void RunAway()
    {
        crowLife.RunAway();
    }

    private IEnumerator GoToPumpkin()
    {
        EatSpot randomSpot = null;
        if (targetPumpkin.leftSpot.crow == null && targetPumpkin.rightSpot.crow == null)
        {
            randomSpot = Random.value < .5f
                ? targetPumpkin.leftSpot
                : targetPumpkin.rightSpot;
        }
        else
        {
            randomSpot = targetPumpkin.leftSpot.crow == null
                ? targetPumpkin.leftSpot
                : targetPumpkin.rightSpot;
        }
        
        randomSpot.crow = this;
        var targetPos = randomSpot.eatSpot;

        FixDirection(targetPos);
        
        while (Vector2.SqrMagnitude(targetPos.position - transform.position) >= .0005f)
        {
            if (targetPos == null)
                yield break;
            
            var newPos = Vector2.Lerp(transform.position, targetPos.position, GameSettings.Instance.crow.speed * Time.deltaTime);

            transform.position = newPos;

            yield return null;
        }

        transform.position = targetPos.position;
        crowLife.ReachPumpkin(targetPumpkin);
    }

    public void FixDirection(Vector2 targetPosition)
    {
        spriteRenderer.flipX = (targetPosition.x - transform.position.x) < 0;
    }

    public void FixDirection(Transform target)
    {
        FixDirection(target.position);
    }
    
    #endregion
}