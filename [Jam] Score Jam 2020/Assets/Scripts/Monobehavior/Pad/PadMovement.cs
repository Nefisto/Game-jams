using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class PadMovement : LazyBehavior
{
	[SerializeField] private FloatReference speed;
	[SerializeField] private float speedUpgradeAmount = 1f;
	[SerializeField] private float increaseSizeAmount = .2f;
	
	[SerializeField, Required] private Transform xMinBoundary, xMaxBoundary;
	
	[SerializeField, Required] private InputManager refToInput;
	[ReadOnly] public float dir;
	
	private void Start ()
	{
		refToInput.inputPlayer.Gameplay.Movement.performed += context => dir = context.ReadValue<float>();
	}
	
	private void FixedUpdate ()
	{
		var newPos = new Vector2(dir * speed * Time.fixedDeltaTime, 0f) + (Vector2) transform.position;
		
		newPos.x = Mathf.Clamp(newPos.x, xMinBoundary.position.x, xMaxBoundary.position.x);
		
		rigidbody2D.MovePosition(newPos);
	}

	public void StopMovement ()
	{
		dir = 0f;
	}

	public void IncreaseSpeed ()
	{
		speed.Value += speedUpgradeAmount;
	}

	public void IncreaseSize ()
	{
		var transform1 = transform;
		var currentScale = transform1.localScale;
		currentScale.x += increaseSizeAmount;

		transform1.localScale = currentScale;
	}
}