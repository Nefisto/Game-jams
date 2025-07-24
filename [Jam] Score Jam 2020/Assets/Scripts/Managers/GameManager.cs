using System;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class GameManager : MonoBehaviour
{
	[Title("Variables")]
	[SerializeField] private IntReference playerLife;

	[Title("Event Triggers")]
	[SerializeField] private GameEvent gameOver;
	[SerializeField] private GameEvent gameStart;
	[SerializeField] private GameEvent startFirstStage;

	public RuntimeSet bricksInStage;
	
	private void Start ()
	{
		startFirstStage.Raise();
	}

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			foreach (var bricks in bricksInStage.Items)
			{
				bricks.GetComponent<Breakable>().TakeDamage(10, false);
			}
		}
	}

	public void RemoveLife ()
	{
		playerLife.Value -= 1;

		if (playerLife >= 1)
			return;

		gameOver?.Raise();
	}
	
}