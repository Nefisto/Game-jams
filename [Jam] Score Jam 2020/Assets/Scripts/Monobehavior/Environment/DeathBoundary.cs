using Sirenix.OdinInspector;
using UnityEngine;

public class DeathBoundary : MonoBehaviour
{
	[SerializeField, Required] private GameEvent onDie;

	private void OnTriggerEnter2D (Collider2D other)
	{
		onDie?.Raise();
	}
}