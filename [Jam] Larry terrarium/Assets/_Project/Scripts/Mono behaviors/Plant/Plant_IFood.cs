using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public partial class Plant
{
    [TitleGroup("Debug/Debug", "IFood")]
    [ReadOnly]
    [ShowInInspector]
    public List<IEater> PotentialEaters { get; set; } = new();
    
    public bool IsMeat => false;
    public int EnergyProvided => CurrentEnergy;
    public Action OnDestroyed { get; set; }
    public Action OnBeConsumed { get; set; }

    public void RemovePotentialEater (IEater eater)
    {
        PotentialEaters.Remove(eater);
    }

    public void RegisterPotentialEater (IEater eater)
    {
        PotentialEaters.Add(eater);
    }

    public void RemoveAllEater()
    {
        PotentialEaters.Clear();
    }

    public void MakePotentialEaterToGiveUp()
    {
        foreach (var possibleEater in PotentialEaters.ToList())
            possibleEater.GiveUpFood();
    }

    public void DisableDetectors()
    {
        detectors.ForEach(c => c.enabled = false);
    }

    [Button]
    [DisableInEditorMode]
    public void BeConsumed()
    {
        IsDead = true;
        OnBeConsumed?.Invoke();
        
        MakePotentialEaterToGiveUp();
        DisableDetectors();
        ChangeState(DyingState);
    }

    public void Kill()
    {
        
    }

    public void DisablePhysics()
    {
        
    }
}