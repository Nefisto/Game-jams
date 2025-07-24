public interface IGatherable : IMonobehavior
{
    public bool CanBeGathered { get; }
    public int EnergyProvide { get; }
    
    public void Gather();
}