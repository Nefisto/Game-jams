using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI lifeUI;
	public IntReference lifes;

	private void Awake ()
	{
		lifes.Variable.AfterChangeValue += UpdateLife;
	}

	private void UpdateLife (int currentLifes)
	{
		lifeUI.text = currentLifes.ToString();
	}
}