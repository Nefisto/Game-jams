
using UnityEngine;

[CreateAssetMenu(fileName = "DEBUG", menuName = "DEBUG", order = 0)]
public class DEBUG : ScriptableObject
{
	public void Print (string msg)
	{
		Debug.Log(msg);
	}
}