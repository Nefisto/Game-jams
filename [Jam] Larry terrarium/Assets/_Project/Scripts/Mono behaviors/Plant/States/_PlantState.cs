using System;
using System.Collections;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class PlantState : State
{
    [TitleGroup("Settings")]
    [MinMaxSlider(1, 20, true)]
    [SerializeField]
    protected Vector2Int energyGainRate = new(1, 3);

    [TitleGroup("Settings")]
    [MinMaxSlider(0, 6, true)]
    [SerializeField]
    protected Vector2Int seedsGainOnGrow = new(0, 0);

    [TitleGroup("Settings")]
    [MinMaxSlider(0.5f, 2f, true)]
    [SerializeField]
    protected Vector2 gapBetweenEnergyGain = new(1f, 1.5f);

    [field: TitleGroup("Settings")]
    [field: MinMaxSlider(0, 200, true)]
    [field: SerializeField]
    public virtual Vector2Int EnergyProvided { get; set; }

    [field: TitleGroup("Settings")]
    [field: Range(0, 100)]
    [field: SerializeField]
    public int EnergyNecessaryToGrown { get; set; } = 10;

    [field: TitleGroup("Settings")]
    [field: Range(0, 100)]
    [field: SerializeField]
    public int EnergyNecessaryToShrunk { get; set; } = 5;

    [TitleGroup("Settings")]
    [SerializeField]
    protected Sprite stageSprite;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public virtual bool IsEdible { get; set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public virtual bool IsExtractable { get; set; }
    
    [TitleGroup("Debug")]
    [ShowInInspector]
    public abstract int Level { get; }

    protected NTask checkForLevelChangeRoutine;

    protected NTask gainingEnergyRoutine;

    public Plant Plant => machine as Plant;

    public override void Enter()
    {
        Plant.UpdateSprite(stageSprite);
        Plant.SetEdibleState(IsEdible);
        Plant.AddSeeds(seedsGainOnGrow.GetRandom());

        gainingEnergyRoutine = new NTask(GainingEnergyRoutine());
        checkForLevelChangeRoutine = new NTask(CheckForLevelChangeRoutine());
    }

    public override void Exit()
    {
        gainingEnergyRoutine.Stop();
        checkForLevelChangeRoutine.Stop();
    }

    protected virtual IEnumerator CheckForLevelChangeRoutine()
    {
        while (true)
        {
            if (Plant.CurrentEnergy >= EnergyNecessaryToGrown)
                if (TryLevelUp())
                    break;

            if (Plant.CurrentEnergy <= EnergyNecessaryToShrunk)
                if (TryLevelDown())
                    break;

            yield return new WaitForSeconds(0.5f);
        }
    }

    protected virtual IEnumerator GainingEnergyRoutine()
    {
        while (true)
        {
            var energyGained = energyGainRate.GetRandom();
            Plant.AddEnergy(energyGained, GetLightMultiplier());

            yield return new WaitForSeconds(gapBetweenEnergyGain.GetRandom());
            
            if (Plant.IsDead)
                break;
        }
    }

    protected virtual float GetLightMultiplier()
    {
        var validLights = GameObjectsProvider
            .GameLights
            .Where(l => l.IsOn)
            .Where(CheckDistanceWithLight)
            .ToList();

        if (!validLights.Any())
            return 0f;

        return validLights.Aggregate(.5f,
            (multiplier, light) => multiplier
                                   + (1f - DistanceFromLight(light) / Plant.MaxDistanceFromLight),
            f => Mathf.Min(f, 1.5f));
    }

    private bool CheckDistanceWithLight (ILight l) => DistanceFromLight(l) < Plant.MaxDistanceFromLight;
    private float DistanceFromLight (ILight l) => Mathf.Abs(l.Transform.position.x - Plant.transform.position.x);

    public abstract bool TryLevelUp();
    public abstract bool TryLevelDown();
}