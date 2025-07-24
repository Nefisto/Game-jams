using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator animator;

    public void PlayGame() 
    {
        Debug.Log("Play");
        SceneManager.LoadScene("Controllers");
    }

    public void QuitGame () 
    {
        Debug.Log("Quit");
        Application.Quit();
    }


    public void addCount()
    {
        var count = animator.GetInteger("count");
        Debug.Log(count);
        animator.SetInteger("count",++count);
    }

}
