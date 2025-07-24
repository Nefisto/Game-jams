using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{    
	public void ChangeToScene(int sceneNumber)    
	{        
		SceneManager.LoadScene(sceneNumber);    
	}
}
