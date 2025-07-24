using System;
using System.Collections;

[Serializable]
public class Grown2 : PlantState
{
    public override int Level => 3;

    public override bool TryLevelUp()
    {
        Plant.ChangeState(Plant.Grown3State);
        return true;
    }

    public override bool TryLevelDown()
    {
        Plant.ChangeState(Plant.Grown1State);
        return true;
    }
}