using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                ResumeState();
            }
            else {
                PauseState();
            }
        }    
    }

    public void ResumeState() 
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void PauseState() 
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void MenuPlay() 
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }
    public void QuitPlay() 
    {
        Application.Quit();
    }
}
