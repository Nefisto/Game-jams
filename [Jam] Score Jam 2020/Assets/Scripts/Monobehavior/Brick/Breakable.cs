
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : LazyBehavior
{
	public static int numberOfBreakables = 0;

	[Range(1, 5), OnValueChanged("UpdateCurrentLife"), DisableInPlayMode, SerializeField]
	private int maxLife;

	[ProgressBar(0, "$maxLife"), SerializeField] private int currentLife;
	[SerializeField] private int brickScore = 100;
	
	[Title("Triggers")] 
	[SerializeField] private GameEventInt updateScore;
	[SerializeField] private GameEvent onBreak;
	[SerializeField] private GameEvent destroyedAllBricks;
	
	[Title("Sounds")] 
	[SerializeField] private AudioEvent onHitEffect;
	[SerializeField] private AudioEvent onBreakEffect;
	
	public List<Sprite> damagedSprites;

	public GameObject upgradeItemPrefab;
	public int dropPercent = 3;

	private void OnEnable ()
	{
		numberOfBreakables++;
	}

	private void OnDisable ()
	{
		numberOfBreakables--;

		if (numberOfBreakables == 0)
			destroyedAllBricks.Raise();
	}

	[Button(ButtonStyle.FoldoutButton), DisableInEditorMode]
	public void TakeDamage (int damage = 1, bool giveScore = true)
	{
		currentLife--;

		spriteRenderer.sprite = damagedSprites?[currentLife];
		
		if (currentLife <= 0)
			StartCoroutine(Die(giveScore));
		else
			onHitEffect.Play(audioSource);
	}

	private IEnumerator Die (bool giveScore = true)
	{
		spriteRenderer.enabled = false;
		boxCollider2D.enabled = false;
		onBreakEffect.Play(audioSource);
		
		if (giveScore)
			updateScore?.Raise(brickScore * maxLife);

		if (Random.Range(0, 10) < dropPercent)
			Instantiate(upgradeItemPrefab, transform.position, quaternion.identity);

		onBreak.Raise();
		
		yield return null;
		
		gameObject.SetActive(false);
	}

	// Called to update currentLife progress bar in editor only
	private void UpdateCurrentLife (int newMaxLife)
	{
		currentLife = maxLife;
	}
}
