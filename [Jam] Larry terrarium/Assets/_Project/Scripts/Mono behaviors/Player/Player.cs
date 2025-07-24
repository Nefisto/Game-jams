using System.Collections;
using System.Linq;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    private static readonly int isMoving = Animator.StringToHash("IsMoving");

    [TitleGroup("Settings")]
    [SerializeField]
    private float jumpHeight = 2f;

    [TitleGroup("Settings")]
    [SerializeField]
    private float speed = 5;

    [TitleGroup("References")]
    [SerializeField]
    private Transform carryPosition;

    [TitleGroup("References")]
    [SerializeField]
    private Transform interactableDetector;

    [TitleGroup("References")]
    [SerializeField]
    private Animator animator;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter lightContactSfx;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter generatorContactSfx;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter plantContactSfx;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter feetSfx;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter onJumpSfx;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter onTouchGroundSfx;

    [TitleGroup("SFX")]
    [SerializeField]
    private StudioEventEmitter gatherPlantSfx;

    [TitleGroup("Detections")]
    [SerializeField]
    private Transform feetCollider;

    [TitleGroup("Detections")]
    [SerializeField]
    private Transform frontCollider;

    [TitleGroup("Detections")]
    [SerializeField]
    private ColliderPropagation lightSwitchDetector;

    [TitleGroup("Detections")]
    [SerializeField]
    private ColliderPropagation plantDetector;

    [TitleGroup("Detections")]
    [SerializeField]
    private ColliderPropagation generatorDetector;

    private readonly Collider2D[] frontContactCollider = new Collider2D[5];

    private readonly Collider2D[] groundContactCollider = new Collider2D[5];
    private readonly Collider2D[] interactableCache = new Collider2D[20];

    [TitleGroup("Debug")]
    [ShowInInspector]
    private int groundsDetectedThisTurn;

    private bool isEnabled;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private IGatherable itemBeingCarried;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private bool onGround;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private bool wasWalkingLastFrame;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Vector2 Velocity { get; set; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    public bool IsFalling => Velocity.y < 0f;

    private bool canAcceptInput;
    
    private EventInstance plantSfxInstance;
    private EventInstance gatherPlantSfxInstance;

    private void Awake()
    {
        lightSwitchDetector.OnEnterContact += EnterOnContactWithLightSwitcher;
        generatorDetector.OnEnterContact += EnterContactWithGenerator;
        plantDetector.OnEnterContact += EnterOnContactWithPlant;

        GameEvents.OnGameStart += () =>
        {
            isEnabled = true;
            plantSfxInstance = RuntimeManager.CreateInstance(plantContactSfx.EventReference);
            gatherPlantSfxInstance = RuntimeManager.CreateInstance(gatherPlantSfx.EventReference);
        };

        GameEvents.OnUnlockPlayerMovement += () => canAcceptInput = true;
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        groundsDetectedThisTurn = Physics2D.OverlapBoxNonAlloc(feetCollider.position, feetCollider.localScale, 0f,
            groundContactCollider, LayerMask.GetMask("Ground"));

        if (groundsDetectedThisTurn == 0)
            onGround = false;

        MovementAndGravity();

        var shouldJump = Input.GetAxisRaw("Vertical") > 0;
        if (canAcceptInput && onGround && shouldJump)
        {
            var verticalForce = Mathf.Sqrt(jumpHeight * Physics2D.gravity.y * -2f);
            animator.Play("jump_up");
            onJumpSfx.Play();
            Velocity = new Vector2(Velocity.x, verticalForce);
            onGround = false;
        }

        FrontMovementValidation();

        CheckForInteractionInput();

        var isWalking = Velocity.x != 0;
        RuntimeManager.StudioSystem.setParameterByName("idle", isWalking ? 1f : 0f);

        transform.Translate(Velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        var isWalking = Velocity.x != 0;
        RuntimeManager.StudioSystem.setParameterByName("idle", isWalking ? 1f : 0f);

        if (isWalking != wasWalkingLastFrame)
        {
            if (isWalking)
                feetSfx.Play();
            else
                feetSfx.Stop();
        }

        wasWalkingLastFrame = isWalking;
    }

    private void CheckForInteractionInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (!IsCarryingGatherable() && CheckForGatherable(out var foundGatherable))
        {
            StartCoroutine(SetGathered(foundGatherable));

            return;
        }

        if (IsCarryingGatherable() && CheckRechargeable(out var foundRechargeable))
            StartCoroutine(InteractWithMachine(foundRechargeable));

        CheckForInteractable()?.Interact();
    }

    public IEnumerator InteractWithMachine (IRechargeable foundRechargeable)
    {
        DisableUpdate();
        var moveTween = itemBeingCarried.Transform.DOMove(foundRechargeable.DeliverPosition.position, .5f);

        yield return moveTween.WaitForCompletion();

        foundRechargeable.Recharge(new IRechargeable.RechargeSettings { amount = itemBeingCarried.EnergyProvide });

        Destroy(itemBeingCarried.Transform.gameObject);
        itemBeingCarried = null;
        EnableUpdate();
    }

    private bool CheckRechargeable (out IRechargeable rechargeable)
    {
        rechargeable = null;
        var amount = Physics2D.OverlapBoxNonAlloc(interactableDetector.position, interactableDetector.localScale,
            0f,
            interactableCache, LayerMask.GetMask("Rechargeable"));

        if (amount == 0)
            return false;

        rechargeable = interactableCache
            .Take(amount)
            .OrderBy(c => Vector2.Distance(c.transform.position, transform.position))
            .First()
            .GetComponentInParents<IRechargeable>(Extension.Self.Exclude);

        return true;
    }

    private IEnumerator SetGathered (IGatherable foundGatherable)
    {
        gatherPlantSfxInstance.start();

        DisableUpdate();
        foundGatherable.Gather();
        animator.Play("idle");

        var moveTween = foundGatherable.Transform.DOMove(carryPosition.position, .5f);
        yield return moveTween.WaitForCompletion();

        itemBeingCarried = foundGatherable;
        itemBeingCarried.Transform.parent = carryPosition;
        EnableUpdate();
    }

    private void EnableUpdate()
    {
        isEnabled = true;
    }

    private void DisableUpdate()
    {
        isEnabled = false;
        animator.SetBool(isMoving, false);
    }

    private bool IsCarryingGatherable() => itemBeingCarried != null;

    [Button]
    [DisableInEditorMode]
    private void RemoveCarryItem()
    {
        itemBeingCarried.Transform.parent = null;
        Destroy(itemBeingCarried.Transform.gameObject);
        itemBeingCarried = null;
    }

    private bool CheckForGatherable (out IGatherable gatherable)
    {
        gatherable = null;
        var amount = Physics2D.OverlapBoxNonAlloc(interactableDetector.position, interactableDetector.localScale,
            0f,
            interactableCache, LayerMask.GetMask("Gatherable"));

        if (amount == 0)
            return false;

        gatherable = interactableCache
            .Take(amount)
            .OrderBy(c => Vector2.Distance(c.transform.position, transform.position))
            .Select(c => c.GetComponentInParents<IGatherable>(Extension.Self.Exclude))
            .FirstOrDefault(g => g.CanBeGathered);

        if (gatherable is not null)
            gatherPlantSfxInstance.setParameterByName("growth", (gatherable as IPlant)?.GetGrowthLevel ?? -1);

        return gatherable != null;
    }

    private IInteractable CheckForInteractable()
    {
        var amount = Physics2D.OverlapBoxNonAlloc(interactableDetector.position, interactableDetector.localScale,
            0f,
            interactableCache, LayerMask.GetMask("Interactable"));

        if (amount == 0)
            return null;

        return interactableCache
            .Take(amount)
            .OrderBy(c => Vector2.Distance(c.transform.position, transform.position))
            .First()
            .GetComponentInParents<IInteractable>(Extension.Self.Exclude);
    }

    private void EnterContactWithGenerator (Collider2D col) => generatorContactSfx.Play();

    private void EnterOnContactWithPlant (Collider2D col)
    {
        var growthLevel = col.GetComponentInParents<IPlant>(Extension.Self.Exclude).GetGrowthLevel;
        if (growthLevel >= 0)
            plantSfxInstance.setParameterByName("growth", growthLevel);

        plantSfxInstance.start();
    }

    private void EnterOnContactWithLightSwitcher (Collider2D col) => lightContactSfx.Play();

    private void FrontMovementValidation()
    {
        var foundWalls = Physics2D.OverlapBoxNonAlloc(frontCollider.position, frontCollider.localScale,
            0f, frontContactCollider,
            LayerMask.GetMask("Wall") | LayerMask.GetMask("Ground"));

        if (foundWalls == 0)
            return;

        Velocity = new Vector2(0f, Velocity.y);
    }

    private void MovementAndGravity()
    {
        var horizontalAxis = canAcceptInput ? Input.GetAxisRaw("Horizontal") : 0f;
        var shouldMove = horizontalAxis != 0;

        animator.SetBool(isMoving, Velocity.x != 0);

        var horizontalVelocity = shouldMove ? speed : 0f;
        if (shouldMove)
        {
            var desiredRotation = horizontalAxis > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, desiredRotation, 0f);
        }

        var verticalVelocity = Velocity.y + Physics2D.gravity.y * Time.deltaTime;

        Velocity = new Vector2(horizontalVelocity, verticalVelocity);

        if (Velocity.y < 0 && groundsDetectedThisTurn > 0)
        {
            Velocity = new Vector2(Velocity.x, 0f);

            var surface = Physics2D.ClosestPoint(transform.position, groundContactCollider[0]);
            transform.position = new Vector2(transform.position.x, surface.y);

            if (onGround)
                return;

            RuntimeManager.StudioSystem.setParameterByName("index_ground",
                (int)groundContactCollider[0].GetComponentInParents<Ground>(Extension.Self.Exclude).GroundType);

            onTouchGroundSfx.Play();
            animator.Play("idle");
            onGround = true;
        }
    }
}