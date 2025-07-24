using System;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Light
{
    public event Action OnTurnedOff;
    public event Action OnTurnedOn;

    [field: TitleGroup("Settings")]
    [field: Range(.5f, 3f)]
    [field: SerializeField]
    public float Radius { get; set; } = 1.5f;

    [TitleGroup("Debug")]
    [ShowInInspector]
    public bool HasEnergy { get; set; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    public bool EnergySwitch { get; private set; } = true;

    [field: TitleGroup("Debug")]
    [field: SerializeField]
    public bool IsOn { get; set; }

    [Button]
    [DisableInEditorMode]
    public void TurnSwitchOn()
    {
        EnergySwitch = true;
        toggleSwitchSfx.Play();

        ValidateState();
        
        lampEnergyCheckSfx.start();
    }
    
    [Button]
    [DisableInEditorMode]
    public void TurnSwitchOff()
    {
        EnergySwitch = false;
        toggleSwitchSfx.Play();
        
        ValidateState();
        
        lampEnergyCheckSfx.start();
    }


    [Button]
    [DisableInEditorMode]
    public bool TurnOn()
    {
        if (IsOn)
            return false;
        
        lampEnergyCheckSfx.setParameterByName("energy", 1f);

        IsOn = true;
        OnTurnedOn?.Invoke();
        
        RefreshVisual();

        return true;
    }

    [Button]
    [DisableInEditorMode]
    public bool TurnOff()
    {
        if(!IsOn)
            return false;
        
        IsOn = false;
        OnTurnedOff?.Invoke();
        
        RefreshVisual();

        return true;
    }
}