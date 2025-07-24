using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class Plant
{

    public Action OnBecomeInedible { get; set; }

    public int GetGrowthLevel => CurrentPlantState.Level;

    public ExtractResult TryExtractEnergy (int energyRequired)
    {
        var result = new ExtractResult();
        if (IsDead)
            return result;
        
        var extractedEnergy = Mathf.Min(energyRequired, CurrentEnergy);

        if (AmountOfSeeds > 0)
        {
            AmountOfSeeds--;
            result.amountOfSeeds++;
        }

        result.extractedEnergy = extractedEnergy;
        
        CurrentEnergy -= extractedEnergy;

        if (CurrentEnergy <= 0)
            BeConsumed();

        return result;
    }

    public class ExtractResult
    {
        public int extractedEnergy;
        public int amountOfSeeds;
    }
}