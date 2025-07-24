using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeItem : LazyBehavior
{
	public GameEvent getUpgrade;
	public GameEvent upgradeDrop;

	public float speed = 3f;

	private int randomizedDir = 1;
	
	private void Start ()
	{
		randomizedDir = Random.Range(0, 2) == 0 ? 1 : -1;
		
		upgradeDrop.Raise();
	}

	private void Update ()
	{
		var dir = Vector2.down * (randomizedDir * speed * Time.deltaTime);
		
		transform.Translate(dir);
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.transform.CompareTag("Player"))
		{
			Destroy(gameObject);
			return;
		}

		getUpgrade.Raise();
		DisableItem();
		Destroy(gameObject, .5f);
	}

	private void DisableItem ()
	{
		spriteRenderer.enabled = false;
		circleCollider2D.enabled = false;
	}
}