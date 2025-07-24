using System;

[Serializable]
public class Grown1 : PlantState
{
    public override int Level => 2;

    public override bool TryLevelUp()
    {
        Plant.ChangeState(Plant.Grown2State);
        return true;
    }

    public override bool TryLevelDown()
    {
        Plant.ChangeState(Plant.SproutState);
        return true;
    }
}