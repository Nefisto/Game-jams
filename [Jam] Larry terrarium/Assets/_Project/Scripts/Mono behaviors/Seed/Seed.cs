using System;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

[SelectionBase]
public class Seed : MonoBehaviour
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    private Vector2 FallingVelocity { get; set; }=new (0f, 10f);

    [TitleGroup("Settings")]
    [SerializeField]
    private Vector2 seedOverlapBoxCheckerSize = new(1f, .5f);

    [TitleGroup("References")]
    [SerializeField]
    private Transform boxDetector;

    [TitleGroup("References")]
    [SerializeField]
    private ContactFilter2D groundFoundContact;

    [TitleGroup("References")]
    [SerializeField]
    private Plant plantPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private StudioEventEmitter touchOnGroundSfx;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool shouldCheckGround = true;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Vector2 plantPosition;
    
    private Collider2D[] foundGrounds = new Collider2D[10];
    private Ground targetGroundToPlant;

    private void Awake() => shouldCheckGround = true;

    private void Start()
    {
        GetPlantPosition();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(plantPosition, .1f);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(plantPosition, seedOverlapBoxCheckerSize);
    }

    private void Update()
    {
        if (shouldCheckGround)
            GroundCheck();
        
        transform.Translate(FallingVelocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        var foundGround = Physics2D.OverlapBox(boxDetector.position, boxDetector.localScale, 0f, groundFoundContact,
            foundGrounds);
        
        if (foundGround == 0)
            return;
        
        shouldCheckGround = false;
        if (targetGroundToPlant.GroundType == GroundType.Ground && !HasOtherPlantsNear())
            SpawnPlant();
        
        Destroy(gameObject, .5f);
    }

    [Button]
    private void SpawnPlant()
    {
        var instance = Instantiate(plantPrefab, plantPosition, quaternion.identity);
        
        instance.Init();
        touchOnGroundSfx.Play();
    }

    [Button]
    private void GetPlantPosition()
    {
        var hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, LayerMask.GetMask("Ground"));
        targetGroundToPlant = hit.collider.GetComponentInParents<Ground>(Extension.Self.Exclude);
        plantPosition = hit.point;
    }

    [Button]
    private bool HasOtherPlantsNear()
    {
        var foundSeed = Physics2D.OverlapBox(plantPosition, seedOverlapBoxCheckerSize, 0f, LayerMask.GetMask("Plant"));
        return foundSeed is not null;
    }
}