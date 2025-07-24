
using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[SelectionBase]
public class ScoreManager : MonoBehaviour
{
	[SerializeField] private IntReference score;

	public TextMeshProUGUI localHighScore;

	private void Start ()
	{
		var personalRecord = PlayerPrefs.GetInt("highscore", 0);

		localHighScore.text = personalRecord.ToString().PadLeft(7, '0');
	}

	[Button, DisableInEditorMode]
	public void IncreasePoint (int value)
	{
		score.Value += value;
	}
}