using System;

[Serializable]
public class Grown3 : PlantState
{
    public override int Level => 4;

    public override bool TryLevelUp() => false;

    public override bool TryLevelDown()
    {
        Plant.ChangeState(Plant.Grown2State);
        return true;
    }
}