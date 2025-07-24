using NTools;
using Sirenix.OdinInspector;

public partial class Plant
{
    [TitleGroup("IGatherable")]
    [ShowInInspector]
    public bool CanBeGathered => CurrentPlantState?.IsExtractable ?? false;
    
    public int EnergyProvide => CurrentPlantState.EnergyProvided.GetRandom();

    public void Gather() => ChangeState(ExtractedState);
}