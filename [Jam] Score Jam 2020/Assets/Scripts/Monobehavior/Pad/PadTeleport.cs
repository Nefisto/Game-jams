using Sirenix.OdinInspector;
using UnityEngine;

public class PadTeleport : LazyBehavior
{
	[SerializeField, Required] private GameEventFloat fixUnicornRotation;

	public GameEvent enableInput;
	public GameEvent disableInput;

	public AudioEvent teleportSound;
	private static readonly int _Teleport = Animator.StringToHash("Teleport");

	public void EnableInput () => enableInput.Raise();
	public void DisableInput () => disableInput.Raise();

	public void TriggerTeleport ()
		=> animator.SetTrigger(_Teleport);
	
	public void Teleport ()
	{
		teleportSound.Play(GetComponent<AudioSource>());
		
		var cachedTransform = transform;
		
		var newPosition = cachedTransform.position;
		newPosition.y *= -1;
		
		// If teleport when ball stick, the ball is pointing to wrong side
		var newRotation = cachedTransform.rotation;
		var zValue = newPosition.y > 0 ? -180f : 0;
		newRotation.eulerAngles = new Vector3(0, 0, zValue);
		
		cachedTransform.position = newPosition;
		cachedTransform.rotation = newRotation;

		fixUnicornRotation?.Raise(zValue);
	}
}