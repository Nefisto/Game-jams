using Sirenix.OdinInspector;
using UnityEngine;

public partial class Plant
{
    [field: TitleGroup("Debug")]
    [field: SerializeField]
    private bool ShouldStartFull { get; set; }

    [Button]
    [DisableInEditorMode]
    private void GoToExtractedLevel() => ChangeState(ExtractedState);

    [Button]
    [DisableInEditorMode]
    private void SetEnergy (int energy = 100) => CurrentEnergy = energy;

    [Button]
    [DisableInEditorMode]
    private void LevelUp()
    {
        CurrentEnergy = CurrentPlantState.EnergyNecessaryToGrown;
        CurrentPlantState?.TryLevelUp();
    }

    [Button]
    [DisableInEditorMode]
    private void LevelDown()
    {
        CurrentPlantState?.TryLevelDown();
    }
}