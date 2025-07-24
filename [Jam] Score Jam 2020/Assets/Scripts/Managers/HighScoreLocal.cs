
using System;
using TMPro;
using UnityEngine;

public class HighScoreLocal : MonoBehaviour
{
	public IntReference score;
	public TextMeshProUGUI prText;

	private void Start ()
	{
		Save();
	}

	public void Save ()
	{
		var savedScore = PlayerPrefs.GetInt("highscore", 0);

		if (score > savedScore)
		{
			PlayerPrefs.SetInt("highscore", score.Value);

			prText.text = "New Record!";
		}
		else
			prText.text = "";
	}
}