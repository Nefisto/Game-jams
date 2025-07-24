using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private void Awake()
    {
        PumpkinLife.OnLosePumpkin += GameOverCheck;
    }

    public void GameOverCheck()
    {
        if (PumpkinManager.AlivePumpkins.Count < 3)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}