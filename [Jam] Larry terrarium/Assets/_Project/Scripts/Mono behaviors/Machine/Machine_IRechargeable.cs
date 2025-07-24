using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Machine
{
    [field: TitleGroup("IRechargeable")]
    [field: SerializeField]
    public Transform DeliverPosition { get; private set; }

    public void Recharge (IRechargeable.RechargeSettings settings)
    {
        UpdateEnergy(settings.amount);
        
        RuntimeManager.StudioSystem.setParameterByName("power", EnergyPercentage * 100);
        PhrasesSfx.Play();
    }
}