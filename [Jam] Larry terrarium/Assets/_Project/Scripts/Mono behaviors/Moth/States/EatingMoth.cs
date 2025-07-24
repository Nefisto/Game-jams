using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EatingMoth : MothState
{
    [TitleGroup("Settings")]
    [MinMaxSlider(1, 25)]
    [SerializeField]
    private Vector2Int amountOfEnergyExtractedByBite;
    
    [TitleGroup("Settings")]
    [MinMaxSlider(1f, 3f)]
    [SerializeField]
    private Vector2 gapBetweenBite = Vector2.one;
    
    private NTask eatingRoutine;
    
    public override void Enter()
    {
        Moth.TargetFood.OnBecomeInedible += BecomingInedibleHandler;
        Moth.TargetFood.OnBeConsumed += FoodDieHandle;
        
        eatingRoutine = new NTask(EatingRoutine());
    }

    public override void Exit()
    {
        Moth.TargetFood.OnBeConsumed -= FoodDieHandle;
        Moth.TargetFood.RemovePotentialEater(Moth);
        
        eatingRoutine.Stop();
        Moth.TargetFood = null;
    }

    private void BecomingInedibleHandler() => Moth.StartCoroutine(FinishingRoutine());

    private void FoodDieHandle() => Moth.StartCoroutine(FinishingRoutine());
    
    IEnumerator FinishingRoutine()
    {
        yield return new WaitForSeconds(.2f);
            
        Moth.ChangeState(Moth.IdleState);
    }
    
    private IEnumerator EatingRoutine()
    {
        while (true)
        {            
            if (Moth.IsFull)
            {
                Moth.GiveUpFood();
                Moth.ChangeState(Moth.IdleState);
                break;
            }

            var extractInfo = Moth.TargetFood.TryExtractEnergy(amountOfEnergyExtractedByBite.GetRandom());
            
            Moth.UpdateHunger(extractInfo.extractedEnergy);
            Moth.UpdateSeeds(extractInfo.amountOfSeeds);
            
            yield return new WaitForSeconds(gapBetweenBite.GetRandom());
        }
    }
}