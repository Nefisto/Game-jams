using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public partial class Machine : MonoBehaviour, IInteractable, IRechargeable
{
    public static event Action OnUpdatedEnergy;
        
    [field: TitleGroup("Settings")]
    [field: Range(50f, 200f)]
    [field: SerializeField]
    public float MaxEnergy { get; private set; } = 100f;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public float RateOfEnergyLoss { get; private set; } = 10f;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    private float GapBetweenEnergyConsume { get; set; } = 1.5f;

    [field: TitleGroup("Settings")]
    [field: Range(0f, 1f)]
    [field: SerializeField]
    public float InitialAmountOfEnergy { get; private set; } = .5f;
    
    [field: TitleGroup("SFX")]
    [field: SerializeField]
    public StudioEventEmitter PhrasesSfx { get; private set; }
    
    [field: TitleGroup("Debug")]
    [field: ProgressBar(0, "MaxEnergy")]
    [field: SerializeField]
    public float AmountOfEnergy { get; private set; }

    [TitleGroup("Debug")]
    [ProgressBar(0f, 1f)]
    [ShowInInspector]
    private float EnergyPercentage => AmountOfEnergy / MaxEnergy; 
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private List<ILight> lightPostInOrder = new();
    
    private void Awake() => GameEvents.OnGameStart += Init;

    private void Init()
    {
        AmountOfEnergy = MaxEnergy * InitialAmountOfEnergy;
        lightPostInOrder = GameObjectsProvider
            .GameLights
            .OrderBy(l => l.OrderToBeFed)
            .ToList();

        AdjustEnergy();
        StartCoroutine(EnergyConsumingRoutine());
    }

    private IEnumerator EnergyConsumingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(GapBetweenEnergyConsume);
            
            UpdateEnergy(-RateOfEnergyLoss * Time.deltaTime);
        }
    }

    private void AdjustEnergy()
    {
        var remainingEnergyResource = EnergyPercentage;
        foreach (var l in lightPostInOrder)
        {
            l.HasEnergy = l.EnergyConsumeRate <= remainingEnergyResource;
            remainingEnergyResource -= l.EnergyConsumeRate;
        }
    }
    
    private void UpdateEnergy (float energyAmount)
    {
        AmountOfEnergy = Mathf.Clamp(AmountOfEnergy + energyAmount, 0f, MaxEnergy);
        
        AdjustEnergy();
        OnUpdatedEnergy?.Invoke();
    }
}