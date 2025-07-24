using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Moth
{
    [TitleGroup("IFood")]
    [ReadOnly]
    [ShowInInspector]
    public List<IEater> PotentialEaters { get; set; } = new();
    
    public bool IsMeat => true;
    public int EnergyProvided => 150;
    public Action OnDestroyed { get; set; }
    public Action OnBeConsumed { get; set; }
    public bool IsEdible => true;

    public bool CanApplyPhysics { get; set; } = true;
    
    public void RegisterPotentialEater (IEater eater)
    {
        PotentialEaters.Add(eater);
    }

    public void RemovePotentialEater (IEater eater)
    {
        PotentialEaters.Remove(eater);
    }

    public void RemoveAllEater()
    {
        PotentialEaters.Clear();
    }

    public void MakePotentialEaterToGiveUp()
    {
        
    }

    public void DisableDetectors()
    {
        detectors.ForEach(c => c.enabled = false);
    }

    public void BeConsumed()
    {
        OnBeConsumed?.Invoke();
    }

    public void Kill()
    {
        if (TargetFood == null)
            return;
        
        TargetFood.RemovePotentialEater(this);
        ChangeState(DeadState);
    }

    public void DisablePhysics()
    {
        CanApplyPhysics = false;
    }

    public void GiveUpFood()
    {
        CurrentMothState.GiveUpFood();
    }
}