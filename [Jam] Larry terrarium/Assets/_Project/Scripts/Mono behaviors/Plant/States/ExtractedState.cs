using UnityEngine;

public class ExtractedState : PlantState
{
    public override int Level => cachedState.Level;
    public override Vector2Int EnergyProvided => cachedState.EnergyProvided;
    public override bool IsEdible => false;
    public override bool IsExtractable => false;

    private PlantState cachedState;

    public override void Enter()
    {
        cachedState = Plant.CurrentPlantState;
        Plant.MakePotentialEaterToGiveUp();
        Plant.DisableDetectors();
    }

    public override void Exit()
    {
        
    }

    public override bool TryLevelUp() => false;

    public override bool TryLevelDown() => false;
}