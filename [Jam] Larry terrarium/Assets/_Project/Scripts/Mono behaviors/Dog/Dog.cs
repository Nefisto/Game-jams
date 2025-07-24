using System;
using System.Collections;
using FMODUnity;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public partial class Dog : StateMachine, IEater
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public Vector2 JumpAmount { get; private set; } = new(1.5f, 1.5f);

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public float HungryLossRate { get; private set; } = 10f;
    
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public float MaxHunger { get; private set; } = 200f;

    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(0f, 1f, true)]
    [field: SerializeField]
    public Vector2 InitialHunger { get; set; } = new(0.25f, 0.75f);

    [TitleGroup("Settings")]
    [SerializeField]
    private float minimumToHungry = 100f;

    [field: TitleGroup("References")]
    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: TitleGroup("References")]
    [field: SerializeField]
    public StudioEventEmitter DogEatSfx { get; private set; }

    [TitleGroup("Detections")]
    [SerializeField]
    private Transform feetCollider;

    [field: TitleGroup("States")]
    [field: SerializeField]
    public IdleState IdleState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public WanderingState WanderingState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public LookingForFoodDog LookingForFoodState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public GoingForFoodState GoingForFoodState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public EatingStateDog EatingState { get; private set; }

    [field: TitleGroup("States")]
    [field: SerializeField]
    public ConfuseDogState ConfuseState { get; private set; }

    private readonly Collider2D[] groundContactCollider = new Collider2D[5];

    private bool hasStarted;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public IFood TargetFood { get; set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool shouldJumpThisFrame;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public float CurrentHungry { get; set; }

    [TitleGroup("States")]
    [PropertyOrder(-1)]
    [ShowInInspector]
    private string StateName => CurrentState?.StateName ?? "OFF";

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public Vector2 Velocity { get; private set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public bool OnGround { get; set; }

    public bool IsHungry => CurrentHungry < minimumToHungry;

    public event Action OnTouchGround;

    private IEnumerator hungryConsumeRoutine;
    
    private void Awake()
    {
        IdleState.Setup(new State.Settings
        {
            name = "Idle",
            machine = this
        });

        WanderingState.Setup(new State.Settings
        {
            name = "Wandering",
            machine = this
        });

        LookingForFoodState.Setup(new State.Settings()
        {
            name = "Looking for food",
            machine = this
        });

        GoingForFoodState.Setup(new State.Settings()
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

        GameEvents.OnGameStart += Init;
    }

    private void Update()
    {
        if (!hasStarted)
            return;

        Velocity += new Vector2(0f, Physics2D.gravity.y * Time.deltaTime);

        ValidateGround();

        if (shouldJumpThisFrame)
        {
            Jump();
            shouldJumpThisFrame = false;
        }

        transform.Translate(Velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected() => CurrentState?.OnDrawGizmosSelected();

    public void RequestJump() => shouldJumpThisFrame = true;

    public void SetFood (IFood possibleFood)
    {
        TargetFood = possibleFood;
    }

    private void Init()
    {
        hasStarted = true;
        CurrentHungry = MaxHunger * InitialHunger.GetRandom();
        
        Setup(IdleState);

        hungryConsumeRoutine = HungryConsumeRoutine();
        StartCoroutine(hungryConsumeRoutine);
    }
    
    private void ValidateGround()
    {
        if (Velocity.y < 0f
            && Physics2D.OverlapBoxNonAlloc(feetCollider.position, feetCollider.localScale, 0f,
                groundContactCollider, LayerMask.GetMask("Ground"))
            > 0)
        {
            Velocity = new Vector2(0f, 0f);

            var surface = Physics2D.ClosestPoint(transform.position, groundContactCollider[0]);
            transform.position = new Vector2(transform.position.x, surface.y);

            if (OnGround)
                return;

            OnGround = true;
            OnTouchGround?.Invoke();
        }
    }

    private IEnumerator HungryConsumeRoutine()
    {
        while (true)
        {
            CurrentHungry = Mathf.Clamp(CurrentHungry - HungryLossRate * Time.deltaTime, 0f, MaxHunger);
            
            yield return null;
        }
    }
    
    private void Jump()
    {
        var verticalForce = Mathf.Sqrt(JumpAmount.y * Physics2D.gravity.y * -2f);

        OnGround = false;
        Velocity = new Vector2(0f, verticalForce);
    }
}