using System;
using System.Collections;
using System.Collections.Generic;
using NTools;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public partial class Moth : StateMachine, IEater, IFood
{
    public event Action OnUpdateHungerAmount;
    
    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(2f, 10f, true)]
    [field: SerializeField]
    public Vector2 HeightValidRange { get; set; }= new(3f, 5f);

    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(1f, 150f, true)]
    [field: SerializeField]
    public Vector2 TimeBetweenSpawnSeeds { get; set; } = new(20f, 120f);

    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(15f, 60f)]
    [field: SerializeField]
    public Vector2 LifespanRange { set; get; } = new(30f, 30f);

    [TitleGroup("References")]
    [SerializeField]
    private Transform seedSpawnPosition;

    [TitleGroup("References")]
    [SerializeField]
    private Seed seedPrefab;

    [field: TitleGroup("References")]
    [field: SerializeField]
    public Animator Animator { get; set; }

    [field: TitleGroup("Hunger")]
    [field: SerializeField]
    public int MaxHunger { get; private set; } = 200;

    [field: TitleGroup("Hunger")]
    [field: MinMaxSlider(0.5f, 2f)]
    [field: SerializeField]
    public Vector2 GapBetweenHungerChecks { get; private set; } = Vector2.one;

    [field: TitleGroup("Hunger")]
    [field: SerializeField]
    public int MinimumHungerForStartLookingForFood { get; private set; } = 50;

    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [TitleGroup("References")]
    [SerializeField]
    private List<Collider2D> detectors;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public int CurrentHunger { get; private set; } = 100;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public ILight TargetLight { get; set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public IPlant TargetFood { get; set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public Vector2 ForwardDirection { get; set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int hungerLossThisTurn;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public bool IsHungry => CurrentHunger < MinimumHungerForStartLookingForFood;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public bool IsFull => CurrentHunger == MaxHunger;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int amountOfSeedsCarrying;

    [TitleGroup("Debug")]
    [ProgressBar(0f, 1f)]
    [ReadOnly]
    [ShowInInspector]
    private float cooldownToSeed;

    [TitleGroup("States")]
    [ShowInInspector]
    public string StateName => CurrentState?.StateName ?? "OFF";

    [field: TitleGroup("States")]
    [field: SerializeField]
    public IdleMoth IdleState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public GoingToLightMoth GoingToLightState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public OrbitingLightMoth OrbitingLightState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public LookingForFoodMoth LookingForFoodState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public GoingToFoodMoth GoingToFoodState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public EatingMoth EatingState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public ConfusedState ConfuseState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public DeadMothState DeadState { get; private set; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    public bool CanSpawnSeed { get; set; } = true;

    [TitleGroup("Debug")]
    [ShowInInspector]
    public bool CanIncreaseHungry { get; set; } = true;

    [TitleGroup("Debug")]
    [ShowInInspector]
    public float randomizedLifespan;
    
    private NoneMothState NoneState { get; set; } = new();
    
    private NTask hungerRoutine;
    private NTask spawnSeedRoutine;

    private MothState CurrentMothState => CurrentState as MothState;
    
    private void Awake()
    {
        IdleState.Setup(new State.Settings
        {
            name = "Idle",
            machine = this
        });
        
        GoingToLightState.Setup(new State.Settings()
        {
            name = "Going to light",
            machine = this
        });
        
        OrbitingLightState.Setup(new State.Settings()
        {
            name = "Orbiting light",
            machine = this
        });
        
        LookingForFoodState.Setup(new State.Settings()
        {
            name = "Looking for food",
            machine = this
        });
        
        GoingToFoodState.Setup(new State.Settings()
        {
            name = "Going for food",
            machine = this
        });
        
        EatingState.Setup(new State.Settings()
        {
            name = "Eating",
            machine = this
        });
        
        ConfuseState.Setup(new State.Settings()
        {
            name = "Confuse",
            machine = this
        });

        DeadState.Setup(new State.Settings()
        {
            name = "Dead",
            machine = this
        });
        
        NoneState.Setup(new State.Settings()
        {
            name = "None",
            machine = this
        });
        
        GameEvents.OnGameStart += Init;
    }

    private void OnEnable() => MothSpawn.AmountOfMoths++;

    private void OnDisable() => MothSpawn.AmountOfMoths--;

    public void UpdateHunger (int amount)
    {
        CurrentHunger = Mathf.Clamp(CurrentHunger + amount, 0, MaxHunger);
        
        OnUpdateHungerAmount?.Invoke();
    }

    public void Init()
    {
        amountOfSeedsCarrying = Random.Range(1, 4);
        randomizedLifespan = LifespanRange.GetRandom();
        
        if (Random.value < .5f)
            FaceLeft();
        else
            FaceRight();
        
        Setup(IdleState);
        
        hungerRoutine = new NTask(HungerRoutine());
        spawnSeedRoutine = new NTask(SpawnSeedRoutine());
        StartCoroutine(LifespanConsumeRoutine());
    }

    private IEnumerator LifespanConsumeRoutine()
    {
        yield return new WaitForSeconds(randomizedLifespan);
        
        ChangeState(DeadState);
    }

    private IEnumerator SpawnSeedRoutine()
    {
        while (true)
        {
            if (!CanSpawnSeed)
                break;
            
            if (amountOfSeedsCarrying == 0)
            {
                yield return new WaitForSeconds(2f);
                continue;
            }

            var delayToNextSpawn = TimeBetweenSpawnSeeds.GetRandom();
            cooldownToSeed = 0f;
            var counter = 0f;
            while (cooldownToSeed < 1f)
            {
                counter += Time.deltaTime * CurrentMothState.SpawnSeedSpeedMultiplier;
                cooldownToSeed = counter / delayToNextSpawn;
                yield return null;
            }

            cooldownToSeed = 0f;
            SpawnSeed();
        }
    }

    private IEnumerator HungerRoutine()
    {
        while (true)
        {
            if (!CanIncreaseHungry)
                break;
            
            yield return new WaitForSeconds(GapBetweenHungerChecks.GetRandom());

            hungerLossThisTurn = CurrentMothState.HungerLossRate.GetRandom();
            UpdateHunger(-hungerLossThisTurn);
        }
    }
    
    [Button]
    [DisableInEditorMode]
    private void SpawnSeed()
    {
        Instantiate(seedPrefab, seedSpawnPosition.position, quaternion.identity);
        amountOfSeedsCarrying--;
    }
    
    private void OnDrawGizmosSelected() => CurrentState?.OnDrawGizmosSelected();

    [Button]
    [DisableInEditorMode]
    public void UpdateSeeds (int amount) => amountOfSeedsCarrying += amount;
    
    public void FaceLeft()
    {
        ForwardDirection = -Vector2.right;
        spriteRenderer.flipX = false;
    }

    public void FaceRight()
    {
        ForwardDirection = Vector2.right;
        spriteRenderer.flipX = true;
    }
}