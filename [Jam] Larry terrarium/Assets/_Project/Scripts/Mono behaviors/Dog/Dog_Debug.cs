using Sirenix.OdinInspector;

public partial class Dog
{
    [Button]
    [DisableInEditorMode]
    private void ToNoneState()
    {
        var noneState = new NoneState();
        noneState.Setup(new State.Settings()
        {
            name = "NONE",
            machine = this
        });
        
        ChangeState(noneState);
    }

    [Button]
    [DisableInEditorMode]
    private void ToIdleState()
    {
        ChangeState(IdleState);
    }

    [Button]
    [DisableInEditorMode]
    private void ToLookingForFoodState()
    {
        ChangeState(LookingForFoodState);
    }

    [Button]
    [DisableInEditorMode]
    private void ChangeAnimationTo (string animationName = "idle")
    {
        Animator.Play(animationName);
    }
}