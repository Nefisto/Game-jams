
using System;

[Serializable]
public class SproutState : PlantState
{
    public override int Level => 1;
    
    public override bool TryLevelUp()
    {
        Plant.ChangeState(Plant.Grown1State);

        return true;
    }

    public override bool TryLevelDown() => false;
}