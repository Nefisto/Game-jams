using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public partial class Plant : StateMachine, IPlant, IFood, IGatherable
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public int MaxEnergy { get; private set; } = 100;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public float MaxDistanceFromLight { get; private set; } = 3f;

    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(1, 10)]
    [field: SerializeField]
    public Vector2Int PossibleAmountOfSeeds { get; private set; } = new(2, 6);
    
    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [TitleGroup("References")]
    [SerializeField]
    private List<Collider2D> detectors;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public bool IsEdible { get; private set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int energyGainedOnThisCheck;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float lightMultiplierOnThisCheck;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public int CurrentEnergy { get; private set; }
    
    [TitleGroup("States")]
    [ShowInInspector]
    public string StateName => CurrentState?.StateName ?? "OFF";
    
    [field: TitleGroup("States")]
    [field: SerializeField]
    public SeedState SeedState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public SproutState SproutState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public Grown1 Grown1State { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public Grown2 Grown2State { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public Grown3 Grown3State { get; private set; }
    
    [field: TitleGroup("States")]
    [field: HideInInspector]
    [field: SerializeField]
    public DyingState DyingState { get; private set; }

    public ExtractedState ExtractedState { get; private set; } = new();

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public bool IsDead { get; set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int AmountOfSeeds { get; set; }
    
    public PlantState CurrentPlantState => CurrentState as PlantState;

    private void Awake()
    {
        SeedState.Setup(new State.Settings
        {
            name = "Seed state",
            machine = this
        });

        SproutState.Setup(new State.Settings
        {
            name = "Sprout state",
            machine = this
        });

        Grown1State.Setup(new State.Settings
        {
            name = "Growth 1",
            machine = this
        });

        Grown2State.Setup(new State.Settings
        {
            name = "Growth 2",
            machine = this
        });

        Grown3State.Setup(new State.Settings
        {
            name = "Growth 3",
            machine = this
        });
        
        DyingState.Setup(new State.Settings
        {
            name = "Dying",
            machine = this
        });
        
        ExtractedState.Setup(new State.Settings()
        {
            name = "Extracted",
            machine = this
        });

        GameEvents.OnGameStart += Init;
    }

    public void Init()
    {
        if (ShouldStartFull)
            CurrentEnergy = MaxEnergy;
        
        Setup(SeedState);
    }

    private void OnDrawGizmosSelected()
    {
        CurrentState?.OnDrawGizmosSelected();

        var pos = transform.position;
        var xPos = pos.x;
        var leftLightVision = new Vector2(xPos - MaxDistanceFromLight, pos.y);
        var rightLightVision = new Vector2(xPos + MaxDistanceFromLight, pos.y);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(leftLightVision + Vector2.down, leftLightVision + Vector2.up * 3f);
        Gizmos.DrawLine(rightLightVision + Vector2.down, rightLightVision + Vector2.up * 3f);
    }

    public void SetEdibleState (bool isEdible)
    {
        if (IsEdible == isEdible)
            return;
        
        if (!isEdible)
            OnBecomeInedible?.Invoke();

        IsEdible = isEdible;
    }

    public void UpdateSprite (Sprite sprite) => spriteRenderer.sprite = sprite;

    public void AddEnergy (int energyAmount, float lightMultiplier)
    {
        if (PotentialEaters.Any())
            return;
        
        lightMultiplierOnThisCheck = lightMultiplier;
        energyGainedOnThisCheck = energyAmount;

        CurrentEnergy = Mathf.RoundToInt(Mathf.Min(CurrentEnergy + energyAmount * lightMultiplier, MaxEnergy));
    }

    public void AddSeeds(int amount)
    {
        AmountOfSeeds = Mathf.Clamp(AmountOfSeeds + amount, 0, 5);
    }
}