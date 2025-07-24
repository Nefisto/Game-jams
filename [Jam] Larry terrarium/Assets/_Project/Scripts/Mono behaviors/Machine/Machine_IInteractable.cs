using FMODUnity;
using UnityEngine;

public partial class Machine
{
    public void Interact()
    {
        RuntimeManager.StudioSystem.setParameterByName("power", EnergyPercentage * 100);
        PhrasesSfx.Play();
    }
}