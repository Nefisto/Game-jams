using System;
using TMPro;
using UnityEngine;

public class RemainingPumpkins : MonoBehaviour
{
    public TextMeshProUGUI remainingPumpkinsText;

    private void Awake()
    {
        GameEvents.OnStartGame += UpdateText;

        PumpkinLife.OnLosePumpkin += UpdateText;
    }

    private void UpdateText()
    {
        remainingPumpkinsText.text = PumpkinManager.AlivePumpkins.Count.ToString();
    }
}