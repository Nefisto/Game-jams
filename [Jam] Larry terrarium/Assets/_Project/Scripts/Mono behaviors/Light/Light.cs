using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public partial class Light : MonoBehaviour, ILight, IInteractable
{
    [field: TitleGroup("Settings")]
    [field: Range(0.05f, 0.2f)]
    [field: SerializeField]
    public float EnergyConsumeRate { get; private set; } = 5f;
    
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public int OrderToBeFed { get; set; }

    [TitleGroup("References")]
    [SerializeField]
    private List<GameObject> objectsThatVisuallyRepresentLight;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter lampBuzzEmitter;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter toggleSwitchSfx;
    
    private EventInstance lampBuzzSfxInstance;
    private EventInstance lampEnergyCheckSfx;
    
    private void Awake()
    {
        GameEvents.OnGameStart += Init;
        GameObjectsProvider.GameLights.Add(this);

        Machine.OnUpdatedEnergy += ValidateState;
    }

    private void Init()
    {
        lampEnergyCheckSfx = RuntimeManager.CreateInstance(toggleSwitchSfx.EventReference);
        lampBuzzSfxInstance = RuntimeManager.CreateInstance(lampBuzzEmitter.EventReference);
            
        Machine.OnUpdatedEnergy += ValidateState;
    }

    private void OnValidate() => RefreshVisual();

    private void ValidateState()
    {
        lampBuzzSfxInstance.setParameterByName("energy", HasEnergy ? 1f : 0f);
        lampEnergyCheckSfx.setParameterByName("energy", HasEnergy ? 1f : 0f);
        
        if (!HasEnergy)
        {
            TurnOff();
            return;
        }

        if (!EnergySwitch)
        {
            TurnOff();
            
            return;
        }
        
        TurnOn();
    }
    
    private void RefreshVisual() => objectsThatVisuallyRepresentLight.ForEach(go => go.SetActive(IsOn));
}