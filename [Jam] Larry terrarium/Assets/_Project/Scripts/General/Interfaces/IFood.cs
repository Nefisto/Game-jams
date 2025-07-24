using System;
using System.Collections.Generic;

public interface IFood : IMonobehavior
{
    public List<IEater> PotentialEaters { get; set; }
    
    public bool IsMeat { get; }
    public int EnergyProvided { get; }
    
    public Action OnDestroyed { get; set; }
    public Action OnBeConsumed { get; set; }
    
    public bool IsEdible { get; }
    
    public void RegisterPotentialEater (IEater eater);
    public void RemovePotentialEater (IEater eater);
    public void RemoveAllEater();
    
    public void MakePotentialEaterToGiveUp();
    public void DisableDetectors();
    public void BeConsumed();
    public void Kill();
    public void DisablePhysics();
}