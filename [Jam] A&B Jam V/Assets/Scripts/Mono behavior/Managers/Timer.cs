using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static event Action<float> OnUpdateTimer;
    private float timer = 0;

    public float maxTime = 60f;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
        
        InitTimer();    
    }

    private void InitTimer()
    {
        StartCoroutine(StartTimer());
    }
    
    private IEnumerator StartTimer()
    {
        while (timer < maxTime)
        {
            yield return new WaitForSeconds(1f);

            timer += 1;
            
            OnUpdateTimer?.Invoke(timer);
        }
        
        // Finish game
        SceneManager.LoadScene("EndScreen");
    }
}