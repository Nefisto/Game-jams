using Sirenix.OdinInspector;

public partial class Moth
{
    [Button]
    [DisableInEditorMode]
    private void SetHungerTo (int amount)
    {
        var amountToChange = amount - CurrentHunger;
        UpdateHunger(amountToChange);
    }

    [Button]
    [DisableInEditorMode]
    private void GoToIdle()
    {
        ChangeState(IdleState);
    }

    [Button]
    [DisableInEditorMode]
    private void GoToDeadState()
    {
        ChangeState(DeadState);
    }

    [Button]
    [DisableInEditorMode]
    private void GoToNoneState()
    {
        ChangeState(NoneState);
    }
    
    [Button]
    [DisableInEditorMode]
    private void GoToHungry()
    {
        ChangeState(LookingForFoodState);
    }
}