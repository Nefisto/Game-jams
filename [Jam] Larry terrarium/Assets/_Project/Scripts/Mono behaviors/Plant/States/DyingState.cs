using System;
using Object = UnityEngine.Object;

[Serializable]
public class DyingState : PlantState
{
    public override int Level => -1;
    public override bool IsEdible => false;
    public override bool IsExtractable => false;

    public override void Enter()
    {
        Object.Destroy(Plant.gameObject, .5f);
    }

    public override void Exit() { }

    public override bool TryLevelUp() => false;

    public override bool TryLevelDown() => false;
}