
using UnityEngine;

public class BallPower : MonoBehaviour
{
	[SerializeField] private IntReference power;
	
	private void OnCollisionEnter2D (Collision2D other)
	{
		if (!other.transform.CompareTag("Brick"))
			return;

		if (other.gameObject.TryGetComponent<Breakable>(out var breakable))
		{
			breakable.TakeDamage(power);
		}
	}

	public void IncreasePower ()
	{
		power.Value++;
	}
}