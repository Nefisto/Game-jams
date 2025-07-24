using System;
using UnityEngine;

public interface ILight : IMonobehavior
{
    public event Action OnTurnedOff;
    public event Action OnTurnedOn;
    
    public bool IsOn { get; }
    public float Radius { get; }
    public bool HasEnergy { get; set; } 
    public int OrderToBeFed { get; }
    public float EnergyConsumeRate { get; }
    public bool EnergySwitch { get; }

    public void TurnSwitchOn();
    public void TurnSwitchOff();

    public bool TurnOn();
    public bool TurnOff();
}