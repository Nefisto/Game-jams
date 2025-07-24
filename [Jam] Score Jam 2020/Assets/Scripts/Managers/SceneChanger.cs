using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneChanger : MonoBehaviour
{	
	public void ChangeScene ()
	{
		SceneManager.LoadScene(Random.Range(2, Constants.NumberOfStages + 1), LoadSceneMode.Additive);
	}

	public void GameOverScreen ()
	{
		SceneManager.LoadScene(7);
	}
}