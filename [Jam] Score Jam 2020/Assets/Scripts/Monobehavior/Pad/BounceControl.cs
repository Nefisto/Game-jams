
using UnityEngine;

// With this the player can slighty "control" to where the ball will go after hit the pad
public class BounceControl : MonoBehaviour
{
	[SerializeField] private float thrust = 0.00001f; // Force influence that player can apply

	private void OnCollisionEnter2D (Collision2D other)
	{
		var contact = other.GetContact(0);
		
		var velocity = other.rigidbody.velocity;
		
		// If came from right in x axys
		if (contact.point.x > transform.position.x)
		{
			other.rigidbody.AddForce(Vector2.right * thrust, ForceMode2D.Force);

			other.rigidbody.AddForce(velocity * .1f, ForceMode2D.Force);
		}
		else if (contact.point.x < transform.position.x)
		{
			other.rigidbody.AddForce(Vector2.left * thrust, ForceMode2D.Force);

			other.rigidbody.AddForce(velocity * .1f, ForceMode2D.Force);
		}
	}

}