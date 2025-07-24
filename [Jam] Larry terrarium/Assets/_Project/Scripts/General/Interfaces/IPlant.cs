using System;

public interface IPlant : IFood
{
    public Action OnBecomeInedible { get; set; }
    public int GetGrowthLevel { get; }
    
    public Plant.ExtractResult TryExtractEnergy (int energyRequired);
}