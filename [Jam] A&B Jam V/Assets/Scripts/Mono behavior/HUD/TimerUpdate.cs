using System;
using TMPro;
using UnityEngine;

public class TimerUpdate : MonoBehaviour
{
    public TextMeshProUGUI timerTMP;

    private void Awake()
    {
        Timer.OnUpdateTimer += UpdateTimer;
    }

    public void UpdateTimer (float time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);

        timerTMP.text = timeSpan.ToString("mm':'ss");
    }
}