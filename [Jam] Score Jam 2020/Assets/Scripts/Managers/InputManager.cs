using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class InputManager : MonoBehaviour
{
	[Title("Trigger events")]
	[SerializeField] private GameEvent onTeleport;
	[SerializeField] private GameEvent onPause;
	[SerializeField] private GameEvent onResume;
	[SerializeField] private GameEvent onUseUpgrade;
	[SerializeField] private GameEvent onLaunchBall;
	
	public InputPlayer inputPlayer;

	[SerializeField, ReadOnly] private bool isPaused = false;

	private void Awake ()
	{
		inputPlayer = new InputPlayer();

		inputPlayer.Gameplay.Teleport.performed += context => onTeleport?.Raise();
		inputPlayer.Gameplay.Upgrade.performed += context => onUseUpgrade?.Raise();
		inputPlayer.Gameplay.Launch.performed += context =>
		{
			if (BallMovement.isMoving)
				return;

			onLaunchBall?.Raise();
		};
		
		inputPlayer.Gameplay.Pause.performed += context =>
		{
			if (!isPaused)
				onPause?.Raise();
			else
				onResume?.Raise();

			isPaused = !isPaused;
		};
	}

	private void OnEnable ()
	{
		EnableInput();
	}

	private void OnDisable ()
	{
		DisableInput();
	}
	
	public void EnableInput() 
		=> inputPlayer.Enable();
	
	public void DisableInput ()
	{
		inputPlayer.Disable();
		
		inputPlayer.Gameplay.Pause.Enable();
	}

	public void DisableLaunch () => inputPlayer.Gameplay.Launch.Disable();
	public void EnableLaunch () => inputPlayer.Gameplay.Launch.Enable();
}