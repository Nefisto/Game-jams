using UnityEngine;

public interface IRechargeable : IMonobehavior
{
    public Transform DeliverPosition { get; }
    
    public void Recharge (RechargeSettings settings);

    public class RechargeSettings
    {
        public int amount;
    }
}