using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Coroutine cheatModeCoroutine;

    private GameSettings gameSettings;
    
    
    private void Awake()
    {
        gameSettings = GameSettings.Instance;

        // GameEvents.OnStartGame += InitTimer;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
        
        GameEvents.StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(gameSettings.inputs.cheatMode))
        {
            if (cheatModeCoroutine == null)
            {
                cheatModeCoroutine = StartCoroutine(CheatMode());
                GameEvents.OnStartCheatMode?.Invoke();
            }
            else
            {
                StopCoroutine(cheatModeCoroutine);
                cheatModeCoroutine = null;
                GameEvents.OnStopCheatMode?.Invoke();
            }
        }
    }

    private IEnumerator CheatMode()
    {
        while (true)
        {
            GameEvents.OnApplyCheatMode?.Invoke();
            
            yield return new WaitForSeconds(3f);
        }
    }
}