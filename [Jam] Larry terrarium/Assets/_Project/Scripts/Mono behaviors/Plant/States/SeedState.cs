using System;
using Sirenix.OdinInspector;

[Serializable]
public class SeedState : PlantState
{
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool hasGainedInitialBonus;

    public override int Level => 0;

    public override void Enter()
    {
        base.Enter();

        if (hasGainedInitialBonus)
            return;
        
        Plant.AddEnergy(5, 1f);
        hasGainedInitialBonus = true;
    }

    protected override float GetLightMultiplier() => 1f;
    
    public override bool TryLevelUp()
    {
        Plant.ChangeState(Plant.SproutState);
        return true;
    }

    public override bool TryLevelDown() => false;
}