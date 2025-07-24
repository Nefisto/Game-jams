using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{ 
	public List<Image> myButtons;

	public IntReference upgradeCounter;
	
	public Sprite activedImage;
	public Sprite desactivedImage;
	public Sprite blockedImage;

	public void ChangeActivedButton ()
	{
		StartCoroutine(_ChangedActiveButton());
		
		IEnumerator _ChangedActiveButton ()
		{
			foreach (var upgradeButton in myButtons)
			{
				upgradeButton.sprite = desactivedImage;
			}

			if (upgradeCounter.Value == 0)
				yield break;

			myButtons[upgradeCounter.Value - 1].sprite = activedImage;
		}
	}
}