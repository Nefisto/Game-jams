using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class UpgradeManager : MonoBehaviour
{
	[Title("Variables")] 
	[SerializeField] private IntReference upgradeCounter;

	[Title("Triggers")] 
	[SerializeField] private GameEvent powerUpgrade;
	[SerializeField] private GameEvent speedUpgrade;
	[SerializeField] private GameEvent sizeUpgrade;
	[SerializeField] private GameEvent failUpgrade;

	private int powerUpgradesCounter = 0;
	private int speedUpgradeCounter = 0;
	private int sizeUpgradeCounter = 0;
	
	[Button]
	public void ApplyUpgrade ()
	{
		switch (upgradeCounter)
		{
			case 1:
				if (speedUpgradeCounter >= Constants.MaxNumberOfSpeedUpgrade)
				{
					failUpgrade?.Raise();
					break;
				}
				
				speedUpgrade?.Raise();
				speedUpgradeCounter++;

				upgradeCounter.Value = 0;
				
				break;
			
			case 2:
				if (sizeUpgradeCounter >= Constants.MaxNumberOfSizeUpgrade)
				{
					failUpgrade?.Raise();
					break;
				}

				sizeUpgrade?.Raise();
				sizeUpgradeCounter++;

				upgradeCounter.Value = 0;

				break;

			case 3:
				if (powerUpgradesCounter >= Constants.MaxNumberOfPowerUpgrades)
				{
					failUpgrade?.Raise();
					break;
				}

				powerUpgrade?.Raise();
				powerUpgradesCounter++;

				upgradeCounter.Value = 0;
				
				break;

			default:
				failUpgrade?.Raise();
				break;

		}
	}

	[Button]
	public void GetUpgrade ()
	{
		upgradeCounter.Value = 1 + (upgradeCounter.Value % Constants.NumberOfUpgrades);
	}
}