
using System;
using TMPro;
using UnityEngine;

public class ScoreUpdateEndScreen : MonoBehaviour
{
	public IntReference score;

	private void Start ()
	{
		GetComponent<TextMeshProUGUI>().text = score.ToString().PadLeft(7, '0');
	}
}