
using UnityEngine;

public class RemoveLastObj : MonoBehaviour
{
	public void DelayiedDestroy ()
	{
		Destroy(gameObject, 1f);
	}
}