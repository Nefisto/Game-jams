
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class BrickController : MonoBehaviour
{
	public enum BrickLevel {Level1, Level2, Level3, Level4, Level5}
	
	[OnValueChanged("UpdateBrick")] public BrickLevel brickLevel = BrickLevel.Level1;
	
	public void UpdateBrick ()
	{
		foreach (Transform child in transform)
			child.gameObject.SetActive(false);

		transform.GetChild((int)brickLevel).gameObject.SetActive(true);
	}
}