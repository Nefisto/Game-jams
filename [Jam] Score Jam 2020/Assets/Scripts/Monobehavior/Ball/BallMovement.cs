using System.Collections;
//using Ludiq.Peek;
//using Ludiq.PeekCore;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class BallMovement : LazyBehavior
{
	private static readonly int _Speed = Animator.StringToHash("Speed");
	
	public static bool isMoving = false;
	
	[TabGroup("Settings"), SerializeField] private Transform respawnPosition;
	[TabGroup("Settings"), SerializeField] private float secondsToRespawn = 1f;
	[TabGroup("Settings"), SerializeField] private bool startMoving = false;
	[TabGroup("Settings"), SerializeField] private bool manualDirection = false;
	[TabGroup("Settings"), SerializeField, ShowIf("manualDirection")] private Vector2 direction;
	[TabGroup("Settings"), SerializeField, Required] private PadMovement padRef;
	[TabGroup("Settings"), SerializeField] private float angleCorrectRange = 15f;
	[TabGroup("Settings"), SerializeField] private float forceCorrectHor = 2f;
	
	[TabGroup("Triggers", true), SerializeField] private GameEvent lockLaunch;
	[TabGroup("Triggers"), SerializeField] private GameEvent unlockLaunch;
	[TabGroup("Triggers"), SerializeField] private GameEvent loadNewStage;

	[TabGroup("Debug", true), SerializeField] private Vector2 cachedVelocity;
	[TabGroup("Debug"), SerializeField, ReadOnly] private Vector2 tweekAppliedLastCol;
	[TabGroup("Debug"), SerializeField, ReadOnly] private Vector2 tweekHoriz;
	[TabGroup("Debug"), SerializeField, ReadOnly] private string lastHittedSide = "";
	[TabGroup("Debug"), SerializeField] private float angleFromLastCol;

	public float tweekSideValue = .5f;
	public float midRange = .3f;
	public float padInfluenceOnTweek = 1f;
	public float velocity = 2f;
	private void Start ()
	{
		Setup(startMoving);
	}

	private void Update ()
	{
		if (isMoving)
			return;

		var transform1 = transform;
		transform1.position = respawnPosition.position;
	}
 
	//Up and down (y) stagnation of ball: use x velocity tweak to change direction to either left or right
	private Vector2 SuddenXVelcoityTweak()
	{
		var currentAngle = rigidbody2D.velocity.ToDegreeAngle();
		angleFromLastCol = currentAngle;

		var correct = 0f;
		if (currentAngle > 100)
			correct = -180;
		else if (currentAngle < -100)
			correct = 180;
		
		var absAngle = Mathf.Abs(currentAngle + correct);

		// Debug.Log(Mathf.Asin(currentXVelocityAbso));
		if (absAngle < angleCorrectRange)
		{ 
			Debug.Log("Horizontal tweek happened");

			var force = 0f;
			
			if (absAngle < float.Epsilon)
			{
				Debug.Log("Mto reto");
				force = Random.Range(0, 2) == 0 ? -forceCorrectHor : forceCorrectHor;
			}
			else
			{
				force = currentAngle < 0 ? -forceCorrectHor : forceCorrectHor;
			}
			
			//push ball left
			return new Vector2(0f, force);
		}
		
		return Vector2.zero;
	}
	
	private void OnCollisionEnter2D (Collision2D other)
	{
		if (!isMoving)
			return;

		// Tweeks based on pad
		var tweek = new Vector2(0, 0);
		if (other.transform.CompareTag("Player"))
		{
			// Wich side of pad u hit
			var dist = transform.position - other.transform.position;
			if (dist.sqrMagnitude > midRange)
			{
				tweek = dist.x > 0 ? new Vector2(tweekSideValue, 0f) : new Vector2(-tweekSideValue, 0f);

				lastHittedSide = dist.x > 0 ? "Right" : "Left";
			}

			// Current direction of pad
			tweek.x += (padRef.dir * padInfluenceOnTweek);
		}
		
		// For horizontal infinite bounce
		tweekHoriz = SuddenXVelcoityTweak();
		tweek += tweekHoriz;

		var currentNormalizedDirection = (rigidbody2D.velocity + tweek).normalized;
		rigidbody2D.velocity = currentNormalizedDirection * velocity;
		
		animator.SetFloat(_Speed, 2);

		var cachedTransform = transform;
		cachedTransform.eulerAngles = new Vector3(0f, 0f, currentNormalizedDirection.ToDegreeAngle() - 90f);

		tweekAppliedLastCol = tweek;
	}

	private void Setup (bool startMoving = false)
	{
		cachedVelocity = manualDirection
			? direction
			: new Vector2(Random.Range(-.8f, .8f), Random.Range(.3f, 1f));

		rigidbody2D.velocity = Vector2.zero;
		
		isMoving = false;

		animator.SetFloat(_Speed, 0f);
		
		if (!startMoving)
		{
			SetInitialPosition();
		}
		else
		{
			LaunchBall();
		}
	}

	public void Respaw ()
	{
		StartCoroutine(_Respawn());

		IEnumerator _Respawn ()
		{
			lockLaunch?.Raise();
			LockMovement();
			DisableVisual();

			yield return new WaitForSeconds(secondsToRespawn);
			
			unlockLaunch?.Raise();
			Setup();
			EnableVisual();
			
			FixRotation(transform.position.y < 0 ? 0 : 180f);
		}
	}
	
	public void FixRotation (float zDegree)
	{
		if (isMoving)
			return;

		var cachedTransform = transform;
		cachedTransform.eulerAngles = new Vector3(0f, 0f, zDegree);
	}
	
	public void SetInitialPosition ()
	{
		transform.position = respawnPosition.position;
	}

	public void LockMovement ()
	{
		cachedVelocity = rigidbody2D.velocity;
		rigidbody2D.velocity = Vector2.zero;
	}

	public void UnlockMovement ()
	{
		rigidbody2D.velocity = cachedVelocity;
	}

	public void GoToPad ()
	{
		StartCoroutine(_GoToPad());
		
		IEnumerator _GoToPad ()
		{
			lockLaunch.Raise();
			rigidbody2D.velocity = Vector2.zero;
			isMoving = false;
			
			var dirToTarger = (Vector2)(transform.position - padRef.transform.position);
			var angle = dirToTarger.ToDegreeAngle();
			
			var cachedTransform = transform;
			cachedTransform.eulerAngles = new Vector3(0f, 0f, cachedVelocity.ToDegreeAngle() - 90);

			rigidbody2D.velocity = dirToTarger * (velocity * .3f); 
			while ((transform.position - padRef.transform.position).sqrMagnitude > .5f)
			{
				Debug.Log((transform.position - padRef.transform.position));
				yield return new WaitForSeconds(.1f);
			}

			unlockLaunch.Raise();
			FixRotation(transform.position.y < 0 ? 0 : 180f);
			
			loadNewStage.Raise();
		}
	}
	
	public void LaunchBall ()
	{
		if (isMoving)
			return;

		rigidbody2D.velocity = cachedVelocity.normalized * velocity;
		
		var cachedTransform = transform;
		cachedTransform.eulerAngles = new Vector3(0f, 0f, cachedVelocity.ToDegreeAngle() - 90);

		animator.SetFloat(_Speed, 2);

		isMoving = true;
	}

	private void DisableVisual ()
	{
		spriteRenderer.enabled = false;
		circleCollider2D.enabled = false;
	}

	private void EnableVisual ()
	{
		spriteRenderer.enabled = true;
		circleCollider2D.enabled = true;
	}

}
