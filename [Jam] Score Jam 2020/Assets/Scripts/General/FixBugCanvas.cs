using UnityEngine;

public class FixBugCanvas : MonoBehaviour
{
	public IntReference upgradeCounter;

	private void Start ()
	{
		// upgradeCounter.Value = 0;
	}

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.K))
			upgradeCounter.Value = 0;
	}
}